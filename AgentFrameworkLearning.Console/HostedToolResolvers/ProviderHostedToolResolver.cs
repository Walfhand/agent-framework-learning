using AgentFrameworkLearning.Console.Abstractions.Models;
using AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;
using Microsoft.Extensions.AI;

namespace AgentFrameworkLearning.Console.HostedToolResolvers;

public abstract class ProviderHostedToolResolver<TSpec> : IProviderHostedToolResolver
    where TSpec : IHostedToolSpec
{
    public abstract Provider Provider { get; }

    public bool CanResolve(IHostedToolSpec spec) => spec is TSpec;

    public IEnumerable<AITool> Resolve(IHostedToolSpec spec, IToolResolutionContext context) => Resolve((TSpec)spec, context);

    protected abstract IEnumerable<AITool> Resolve(TSpec spec, IToolResolutionContext context);
}
