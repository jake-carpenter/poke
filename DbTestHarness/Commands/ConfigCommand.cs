using DbTestHarness.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class ConfigCommand : Command<BaseSettings>
{
    public override int Execute(CommandContext context, BaseSettings settings, CancellationToken cancellationToken)
    {
        var grid = new Grid()
            .AddColumn(new GridColumn().Alignment(Justify.Right))
            .AddColumn(new GridColumn().Alignment(Justify.Left))
            .AddColumn(new GridColumn().Alignment(Justify.Left))
            .AddRow(
                new Markup("[grey]Group[/]"),
                new Markup("[grey]Instance[/]"),
                new Markup("[grey]Host[/]"));

        var config = settings.GetConfig();
        var groups = new ServerGroups(config.Servers);

        foreach (var (_, group) in groups)
        {
            foreach (var server in group.Servers)
            {
                grid.AddRow(
                    new Markup($"[blue]{server.GroupName}[/]"),
                    new Markup($"[yellow]{server.Instance}[/]"),
                    new Markup($"[white]{server.Host}[/]"));
            }
        }

        AnsiConsole.Write(grid);

        return 0;
    }
}