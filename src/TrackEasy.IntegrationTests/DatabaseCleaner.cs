using Respawn;

namespace TrackEasy.IntegrationTests;

public static class DatabaseCleaner
{
    public static async Task Clean(string connectionString, string schema)
    {
        var options = new RespawnerOptions
        {
            TablesToIgnore =
            [
                "__EFMigrationsHistory",
            ],
            SchemasToInclude = [schema],
            WithReseed = true
        };
        var respawner = await Respawner.CreateAsync(connectionString, options);
        await respawner.ResetAsync(connectionString);
    }
}