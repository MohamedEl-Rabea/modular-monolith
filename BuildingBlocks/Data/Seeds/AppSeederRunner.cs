using Microsoft.Extensions.Logging;

namespace WT.B2C.API.BuildingBlocks.Data.Seeds;

public sealed class AppSeederRunner(IEnumerable<IDataSeeder> seeders, ILogger<AppSeederRunner> logger)
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _initialized;

    public async Task EnsureSeededAsync(CancellationToken cancellationToken = default)
    {
        if (_initialized)
            return;

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_initialized)
                return;

            logger.LogInformation("Starting application seeders...");

            foreach (var seeder in seeders)
            {
                var seederName = seeder.GetType().Name;
                try
                {
                    logger.LogInformation("Running seeder {Seeder}", seederName);
                    await seeder.SeedAsync(cancellationToken);
                    logger.LogInformation("Completed seeder {Seeder}", seederName);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while running seeder {Seeder}", seederName);
                    throw;
                }
            }

            _initialized = true;
            logger.LogInformation("All application seeders completed.");
        }
        finally
        {
            _lock.Release();
        }
    }
}