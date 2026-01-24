using Poke.Commands;
using Poke.Models;

namespace Poke.Infrastructure;

/// <summary>
/// Selects the appropriate runner for a given server type.
/// </summary>
public class RunnerFactory
{
    private readonly Dictionary<ServerType, IRunner> _runnerLookup;

    public RunnerFactory(IEnumerable<IRunner> runners)
    {
        _runnerLookup = runners.ToDictionary(x => x.ServerType);
    }

    /// <summary>
    /// Gets a runner for the provided server and settings.
    /// </summary>
    /// <param name="server">The server to execute.</param>
    /// <param name="settings">The run settings.</param>
    /// <returns>The matching runner.</returns>
    public IRunner GetRunner(Server server, RunSettings settings)
    {
        return !_runnerLookup.TryGetValue(server.Type, out var runner)
            ? throw new InvalidOperationException("No suitable runner found.")
            : runner;
    }
}