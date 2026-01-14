namespace DbTestHarness.Models;

public record UserConfig
{
    public required IReadOnlyCollection<Server> Servers { get; init; }
}