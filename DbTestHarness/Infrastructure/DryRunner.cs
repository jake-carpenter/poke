using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public class DryRunner : IRunner
{
    public async Task<bool> Execute(SqlServer sqlServer)
    {
        await Task.Delay(500);

        return true;
    }
}