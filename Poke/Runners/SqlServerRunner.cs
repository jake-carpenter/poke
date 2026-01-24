using Microsoft.Data.SqlClient;
using Poke.Commands;
using Poke.Infrastructure;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// Executes SQL Server connectivity checks.
/// </summary>
public class SqlServerRunner : IRunner
{
    /// <inheritdoc/>
    public ServerType ServerType => ServerType.SqlServer;

    /// <inheritdoc/>
    public IRunnerFormatter Formatter => new SqlServerFormatter();

    /// <inheritdoc/>
    public async Task<RunResult> Execute(Server server, RunSettings settings)
    {
        if (server is not SqlServer sqlServer)
            throw new InvalidOperationException(
                $"Expected {nameof(SqlServer)} but got {server.GetType().Name}"
            );

        if (settings.DryRun)
            return RunResult.Success();

        await using var connection = new SqlConnection(sqlServer.ConnectionString);

        try
        {
            await connection.OpenAsync();
            await connection.CloseAsync();

            return RunResult.Success();
        }
        catch (Exception ex)
        {
            return RunResult.Failure(ex);
        }
    }
}