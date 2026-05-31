using Microsoft.Extensions.AI;

namespace AgentFrameworkLearning.Console.Abstractions.Models;

public record AgentSpec(
    Provider Provider,
    string Model,
    string Name,
    string Instructions,
    IReadOnlyCollection<IHostedToolSpec> HostedTools,
    IReadOnlyCollection<AIFunction> LocalTools)
{
    
}

public interface IHostedToolSpec
{
    HostedTool Tool { get; }
}

public sealed record CodeInterpreterToolSpec : IHostedToolSpec
{
    public HostedTool Tool => HostedTool.CodeInterpreter;
}

public sealed record ToolApprovalToolSpec : IHostedToolSpec
{
    public HostedTool Tool => HostedTool.ToolApproval;
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
