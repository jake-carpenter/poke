namespace DbTestHarness.Models;

public readonly record struct SqlServer(string Name, string Host)
{
    public string ServerLabel => $"{Host} | {Name,-8}";
    public override string ToString() => $"{Host} | {Name,-8}";
}