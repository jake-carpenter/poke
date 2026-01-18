using Poke.Config;
using Poke.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Poke.Commands;

public class AllCommand(RunnerStatus runnerStatus, ConfigManager configManager)
    : AsyncCommand<RunSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        RunSettings settings,
        CancellationToken cancellationToken
    )
    {
        var config = await configManager.Read(settings.ConfigFile);
        if (config.Servers.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No servers configured.[/]");
            return 1;
        }

        var result = await runnerStatus.Start(config.Servers, settings);

        return result;
    }
}
