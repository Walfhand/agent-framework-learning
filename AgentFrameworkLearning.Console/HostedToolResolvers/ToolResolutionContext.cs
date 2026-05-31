using AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;
using Microsoft.Extensions.AI;

namespace AgentFrameworkLearning.Console.HostedToolResolvers;

public sealed class ToolResolutionContext(IReadOnlyCollection<AIFunction> localTools) : IToolResolutionContext
{
    private readonly HashSet<AIFunction> _resolvedLocalTools = [];

    public IReadOnlyCollection<AIFunction> LocalTools { get; } = localTools;

    public bool IsLocalToolResolved(AIFunction tool) => _resolvedLocalTools.Contains(tool);

    public void MarkLocalToolAsResolved(AIFunction tool) => _resolvedLocalTools.Add(tool);
}
