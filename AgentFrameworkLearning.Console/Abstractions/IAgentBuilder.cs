using AgentFrameworkLearning.Console.Abstractions.Models;
using Microsoft.Agents.AI;

namespace AgentFrameworkLearning.Console.Abstractions;

public interface IAgentBuilder
{
    AIAgent Build(AgentSpec spec);
}