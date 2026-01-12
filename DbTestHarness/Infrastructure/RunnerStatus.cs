using DbTestHarness.Models;
using Spectre.Console;

namespace DbTestHarness.Infrastructure;

public class RunnerStatus(RunnerFactory runnerFactory)
{
    public async Task<int> Start(Server[] servers, Settings settings)
    {
        if (servers.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]No servers selected.[/]");

            return 1;
        }

        var results = new Dictionary<Server, RunResult>();

        await AnsiConsole.Progress()
            .AutoClear(false)
            .HideCompleted(false)
            .Columns(
                new SpinnerColumn(),
                new TaskDescriptionColumn { Alignment = Justify.Left })
            .StartAsync(async ctx =>
            {
                var progressTasks = ApplyTasks(ctx, servers);
                var tasks = servers
                    .Where(s => progressTasks.ContainsKey(s))
                    .Select(server => ExecuteServerAsync(server, progressTasks[server], settings, results));

                await Task.WhenAll(tasks);
            });

        var hasFailure = results.Values.Any(r => !r.Succeeded);

        return hasFailure ? 1 : 0;
    }

    private static Dictionary<Server, ProgressTask> ApplyTasks(ProgressContext ctx, Server[] servers)
    {
        var progressTasks = new Dictionary<Server, ProgressTask>();
        foreach (var server in servers)
        {
            if (server is not SqlServerWithGroup sqlServer)
                continue;

            var description = $"[blue]{sqlServer.GroupName}[/] | [yellow]{sqlServer.Name}[/] | {sqlServer.Host}";

            progressTasks[server] = ctx.AddTask(description, autoStart: true, maxValue: 1);
        }

        return progressTasks;
    }

    private async Task ExecuteServerAsync(
        Server server,
        ProgressTask progressTask,
        Settings settings,
        Dictionary<Server, RunResult> results)
    {
        if (server is not SqlServerWithGroup sqlServer)
            return;

        var runner = runnerFactory.GetRunner(sqlServer, settings);
        var result = await runner.Execute(sqlServer);

        lock (results)
        {
            results[server] = result;
        }

        var (color, symbol) = result.Succeeded
            ? ("green", "✔")
            : ("red", "✘");

        progressTask.Description =
            $"[{color}]{symbol}[/] [blue]{sqlServer.GroupName}[/] | [yellow]{sqlServer.Name}[/] | {sqlServer.Host}";
        progressTask.Value = 1;
        progressTask.StopTask();
    }
}