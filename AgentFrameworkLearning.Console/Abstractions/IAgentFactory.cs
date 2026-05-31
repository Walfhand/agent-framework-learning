using AgentFrameworkLearning.Console.Abstractions.Models;
using Microsoft.Agents.AI;

namespace AgentFrameworkLearning.Console.Abstractions;

public interface IAgentFactory
{
    public Provider Provider { get; }
    AIAgent Create(AgentSpec spec);
}