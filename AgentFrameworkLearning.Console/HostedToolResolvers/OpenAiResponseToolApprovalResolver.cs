using AgentFrameworkLearning.Console.Abstractions.Models;
using AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;
using Microsoft.Extensions.AI;

namespace AgentFrameworkLearning.Console.HostedToolResolvers;

public sealed class OpenAiResponseToolApprovalResolver : ProviderHostedToolResolver<ToolApprovalToolSpec>
{
    public override Provider Provider => Provider.OpenAiResponse;

    protected override IEnumerable<AITool> Resolve(ToolApprovalToolSpec spec, IToolResolutionContext context)
    {
        foreach (var localTool in context.LocalTools)
        {
            context.MarkLocalToolAsResolved(localTool);
            yield return new ApprovalRequiredAIFunction(localTool);
        }
    }
}
