using System.ComponentModel;
using System.Runtime.CompilerServices;
using AgentFrameworkLearning.Console.Abstractions;
using AgentFrameworkLearning.Console.Abstractions.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AgentFrameworkLearning.Console.Agents;

public class ToolApprovalAgent(IAgentBuilder builder)
{
    private const int MaxApprovalRounds = 10;

    [Description("Get the weather for a given location.")]
    static string GetWeather([Description("The location to get the weather for.")] string location)
        => $"The weather in {location} is cloudy with a high of 15°C.";

    private readonly AIAgent _agent = builder.Build(new AgentSpec(Provider.OpenAiResponse, "gpt-4o-mini",
        "ToolApprovalAgent", "You are a helpful assistant.",
        [new ToolApprovalToolSpec()], [AIFunctionFactory.Create(GetWeather)]));
    
    
    public async IAsyncEnumerable<string> RunStreamingAsync(
        string prompt,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await RunWithApprovalsAsync(prompt, cancellationToken);
        if (!string.IsNullOrEmpty(response.Text))
        {
            yield return response.Text;
        }
    }

    private async Task<AgentResponse> RunWithApprovalsAsync(
        string prompt,
        CancellationToken cancellationToken)
    {
        var session = await _agent.CreateSessionAsync(cancellationToken);
        var response = await _agent.RunAsync(prompt, session, cancellationToken: cancellationToken);

        for (var round = 0; round < MaxApprovalRounds; round++)
        {
            var approvalRequests = GetApprovalRequests(response).ToArray();
            if (approvalRequests.Length == 0)
            {
                return response;
            }

            foreach (var approvalRequest in approvalRequests)
            {
                var approved = AskForApproval(approvalRequest);
                var approvalMessage = new ChatMessage(
                    ChatRole.User,
                    [approvalRequest.CreateResponse(approved, null)]);

                response = await _agent.RunAsync(approvalMessage, session, cancellationToken: cancellationToken);
            }
        }

        throw new InvalidOperationException("Too many tool approval rounds.");
    }

    private static IEnumerable<ToolApprovalRequestContent> GetApprovalRequests(AgentResponse response)
    {
        return response.Messages
            .SelectMany(message => message.Contents)
            .OfType<ToolApprovalRequestContent>();
    }

    private static bool AskForApproval(ToolApprovalRequestContent request)
    {
        System.Console.WriteLine();
        System.Console.WriteLine($"Approval required for tool call: {GetToolCallName(request.ToolCall)}");
        System.Console.Write("Approve? [y/N] ");

        var answer = System.Console.ReadLine();
        return answer?.Equals("y", StringComparison.OrdinalIgnoreCase) == true
               || answer?.Equals("yes", StringComparison.OrdinalIgnoreCase) == true;
    }

    private static string GetToolCallName(ToolCallContent toolCall)
    {
        return toolCall is FunctionCallContent functionCall
            ? functionCall.Name
            : toolCall.CallId;
    }
}
