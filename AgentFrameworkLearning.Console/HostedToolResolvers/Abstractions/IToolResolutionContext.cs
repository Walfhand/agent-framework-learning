using Microsoft.Extensions.AI;

namespace AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;

public interface IToolResolutionContext
{
    IReadOnlyCollection<AIFunction> LocalTools { get; }
    bool IsLocalToolResolved(AIFunction tool);
    void MarkLocalToolAsResolved(AIFunction tool);
}
