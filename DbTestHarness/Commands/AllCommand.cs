using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class AllCommand(UserConfig userConfig, RunnerStatus runnerStatus) : AsyncCommand<Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var servers = userConfig.SqlServerGroups
            .SelectMany(group => group.ServerWithGroups)
            .ToArray<Server>();

        var result = await runnerStatus.Start(servers, settings);

        return result;
    }
}