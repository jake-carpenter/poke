using DbTestHarness.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class ConfigCommand(UserConfig userConfig) : Command
{
    public override int Execute(CommandContext context)
    {
        var grid = new Grid()
            .AddColumn(new GridColumn().Alignment(Justify.Right))
            .AddColumn(new GridColumn().Alignment(Justify.Left))
            .AddColumn(new GridColumn().Alignment(Justify.Left))
            .AddRow(
                new Markup("[grey]Group[/]"),
                new Markup("[grey]Name[/]"),
                new Markup("[grey]Host[/]"));

        grid.Expand = false;

        foreach (var group in userConfig.SqlServerGroups)
        {
            foreach (var server in group.Servers)
            {
                grid.AddRow(
                    new Markup($"[blue]{group.Name}[/]"),
                    new Markup($"[yellow]{server.Name}[/]"),
                    new Markup($"[white]{server.Host}[/]"));
            }
        }

        AnsiConsole.Write(grid);

        return 0;
    }
}