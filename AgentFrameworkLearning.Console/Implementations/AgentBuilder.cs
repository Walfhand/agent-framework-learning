using AgentFrameworkLearning.Console.Abstractions;
using AgentFrameworkLearning.Console.Abstractions.Models;
using Microsoft.Agents.AI;

namespace AgentFrameworkLearning.Console.Implementations;

public class AgentBuilder : IAgentBuilder
{
    private readonly IEnumerable<IAgentFactory> _agentFactories;

    public AgentBuilder(IEnumerable<IAgentFactory> agentFactories)
    {
        _agentFactories = agentFactories;
    }
    public AIAgent Build(AgentSpec spec)
    {
        var agentFactory = _agentFactories.Single(af => af.Provider == spec.Provider);
        return agentFactory.Create(spec);
    }
}