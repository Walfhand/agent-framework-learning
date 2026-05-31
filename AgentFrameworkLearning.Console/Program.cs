using AgentFrameworkLearning.Console;
using AgentFrameworkLearning.Console.Agents;
using AgentFrameworkLearning.Console.HostedToolResolvers;
using AgentFrameworkLearning.Console.HostedToolResolvers.Abstractions;
using AgentFrameworkLearning.Console.Implementations;

IProviderHostedToolResolver[] openAiResponseToolResolvers =
[
    new OpenAiResponseCodeInterpreterResolver(),
    new OpenAiResponseToolApprovalResolver()
];

var agentBuilder = new AgentBuilder([new OpenAiResponseFactory(openAiResponseToolResolvers)]);
var codeInterpreterAgent = new CodeInterpreterAgent(agentBuilder);
var approvalAgent = new ToolApprovalAgent(agentBuilder);

await foreach (var text in approvalAgent.RunStreamingAsync("calcul la factoriel de 1000"))
{
    Console.Write(text);
}

await foreach (var text in codeInterpreterAgent.RunStreamingAsync("calcul la factoriel de 1000"))
{
    Console.Write(text);
}
