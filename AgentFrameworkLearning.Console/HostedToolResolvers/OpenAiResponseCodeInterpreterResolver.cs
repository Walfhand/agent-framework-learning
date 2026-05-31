using AgentFrameworkLearning.Console.Abstractions.Models;
using AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;
using Microsoft.Agents.AI.Foundry;
using Microsoft.Extensions.AI;
using OpenAI.Responses;

namespace AgentFrameworkLearning.Console.HostedToolResolvers;

#pragma warning disable OPENAI001
public sealed class OpenAiResponseCodeInterpreterResolver : ProviderHostedToolResolver<CodeInterpreterToolSpec>
{
    public override Provider Provider => Provider.OpenAiResponse;

    protected override IEnumerable<AITool> Resolve(CodeInterpreterToolSpec spec, IToolResolutionContext context)
    {
        yield return FoundryAITool.CreateCodeInterpreterTool(
            new CodeInterpreterToolContainer(
                CodeInterpreterToolContainerConfiguration.CreateAutomaticContainerConfiguration([])));
    }
}
