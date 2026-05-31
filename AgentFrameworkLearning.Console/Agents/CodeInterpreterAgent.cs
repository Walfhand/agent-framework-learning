using System.Runtime.CompilerServices;
using AgentFrameworkLearning.Console.Abstractions;
using AgentFrameworkLearning.Console.Abstractions.Models;
using Microsoft.Agents.AI;

namespace AgentFrameworkLearning.Console.Agents;

public sealed class CodeInterpreterAgent(IAgentBuilder agentFactory)
{
    private readonly AIAgent _agent = agentFactory.Build(new AgentSpec(Provider.OpenAiResponse, "gpt-4o-mini",
        "CodeInterpreter", "You are a helpful assistant that can write and execute Python code.",
        [new CodeInterpreterToolSpec()], []));

    public async IAsyncEnumerable<string> RunStreamingAsync(
        string prompt,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var update in _agent.RunStreamingAsync(prompt, cancellationToken: cancellationToken))
        {
            if (!string.IsNullOrEmpty(update.Text))
            {
                yield return update.Text;
            }
        }
    }
}
