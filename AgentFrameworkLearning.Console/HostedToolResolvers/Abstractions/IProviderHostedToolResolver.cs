using AgentFrameworkLearning.Console.Abstractions.Models;
using Microsoft.Extensions.AI;

namespace AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;

public interface IProviderHostedToolResolver
{
    Provider Provider { get; }
    bool CanResolve(IHostedToolSpec spec);
    IEnumerable<AITool> Resolve(IHostedToolSpec spec, IToolResolutionContext context);
}
