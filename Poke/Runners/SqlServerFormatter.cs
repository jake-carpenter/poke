using Poke.Infrastructure;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// Formats SQL Server status output for the console.
/// </summary>
public class SqlServerFormatter : IRunnerFormatter
{
    /// <inheritdoc/>
    public string FormatInProgressLine(Server server)
    {
        if (server is not SqlServer sqlServer)
            return string.Empty;

        var dataSource = sqlServer.DataSource;
        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {dataSource}";
    }

    /// <inheritdoc/>
    public string FormatResultLine(Server server, RunResult result)
    {
        var (color, symbol) = result.Succeeded ? ("green", "✔") : ("red", "✘");
        if (server is not SqlServer sqlServer)
            return string.Empty;

        var dataSource = sqlServer.DataSource;

        return $"[{color}]{symbol}[/] [blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {dataSource}";
    }

    /// <inheritdoc/>
    public string FormatExceptionLine(Server server)
    {
        if (server is not SqlServer sqlServer)
            return string.Empty;

        var dataSource = sqlServer.DataSource;
        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {dataSource}";
    }

    /// <inheritdoc/>
    public string FormatCreated(Server server)
    {
        if (server is not SqlServer sqlServer)
            return string.Empty;

        var dataSource = sqlServer.DataSource;
        return $"[green]✓[/] Successfully added SQL Server: [yellow]{dataSource}[/] / [yellow]{server.Instance}[/] to group [yellow]{server.GroupName}[/]";
    }
}
