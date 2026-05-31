using AgentFrameworkLearning.Console.Abstractions;
using AgentFrameworkLearning.Console.Abstractions.Models;
using AgentFrameworkLearning.Console.HostedToolResolvers;
using AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Responses;

namespace AgentFrameworkLearning.Console.Implementations;
#pragma warning disable OPENAI001
public class OpenAiResponseFactory(IEnumerable<IProviderHostedToolResolver> toolResolvers) : IAgentFactory
{
    public Provider Provider => Provider.OpenAiResponse;
    private readonly IProviderHostedToolResolver[] _toolResolvers =
        toolResolvers.Where(tr => tr.Provider == Provider.OpenAiResponse).ToArray();

    public AIAgent Create(AgentSpec spec)
    {
        var client = new OpenAIClient("");
        var responsesClient = client.GetResponsesClient();
        return responsesClient.AsAIAgent(
            model: spec.Model,
            instructions: spec.Instructions,
            name: spec.Name,
            tools: ResolveTools(spec).ToArray());
    }

    private IEnumerable<AITool> ResolveTools(AgentSpec spec)
    {
        var context = new ToolResolutionContext(spec.LocalTools);

        foreach (var desiredHostedTool in spec.HostedTools)
        {
            var resolver = _toolResolvers.SingleOrDefault(tr => tr.CanResolve(desiredHostedTool));
            if (resolver is null)
            {
                throw new Exception($"{desiredHostedTool.Tool} Not supported for the provider : {Provider}");
            }

            foreach (var tool in resolver.Resolve(desiredHostedTool, context))
            {
                yield return tool;
            }
        }

        foreach (var localTool in spec.LocalTools)
        {
            if (context.IsLocalToolResolved(localTool))
            {
                continue;
            }

            foreach (var tool in ResolveLocalTool(localTool))
            {
                yield return tool;
            }
        }
    }

    private static IEnumerable<AITool> ResolveLocalTool(AIFunction localTool)
    {
        yield return localTool;
    }
}
