using AgentFrameworkLearning.Console.Abstractions;
using AgentFrameworkLearning.Console.Abstractions.Models;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Responses;

namespace AgentFrameworkLearning.Console.Implementations;
#pragma warning disable OPENAI001
public class OpenAiResponseFactory : IAgentFactory
{
    public Provider Provider => Provider.OpenAiResponse;
    private readonly HostedTool[] _hostedTools = [HostedTool.CodeInterpreter, HostedTool.FileSearch, HostedTool.ToolApproval, HostedTool.WebSearch];
    public AIAgent Create(AgentSpec spec)
    {
        var client = new OpenAIClient("");
        var responsesClient = client.GetResponsesClient();
        return responsesClient.AsAIAgent(
            model: spec.Model,
            instructions: spec.Instructions,
            name: spec.Name,
            tools: ResolveHostedTools(spec.HostedTools.ToArray()).ToArray());
    }

    private IEnumerable<AITool> ResolveHostedTools(HostedTool[] desiredHostedTools)
    {
        var notSupportedTools =
            desiredHostedTools.Where(dht => !_hostedTools.Contains(dht)).ToList();
        if (notSupportedTools.Count != 0)
            throw new Exception($"{string.Join(',', notSupportedTools)} Not supported for the provider : {Provider}");
        foreach (var desiredHostedTool in desiredHostedTools)
        {
            switch (desiredHostedTool)
            {
                case HostedTool.CodeInterpreter:
                    yield return FoundryAITool.CreateCodeInterpreterTool(
                        new CodeInterpreterToolContainer(
                            CodeInterpreterToolContainerConfiguration.CreateAutomaticContainerConfiguration([])));
                    break;
            }
        }
    }
}