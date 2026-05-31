namespace AgentFrameworkLearning.Console.Abstractions.Models;

public record AgentSpec(Provider Provider, string Model, string Name, string Instructions, IReadOnlyCollection<HostedTool> HostedTools)
{
    
}

public enum HostedTool
{
    ToolApproval,
    CodeInterpreter,
    FileSearch,
    WebSearch
}

public enum Provider
{
    OpenAiResponse
}