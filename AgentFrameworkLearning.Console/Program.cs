using AgentFrameworkLearning.Console;
using AgentFrameworkLearning.Console.Implementations;

var agentBuilder = new AgentBuilder([new OpenAiResponseFactory()]);
var codeInterpreterAgent = new CodeInterpreterAgent(agentBuilder);

await foreach (var text in codeInterpreterAgent.RunStreamingAsync("calcul la factoriel de 1000"))
{
    Console.Write(text);
}