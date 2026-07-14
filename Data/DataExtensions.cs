using Microsoft.EntityFrameworkCore;
namespace GameStore.API.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("NeonConnection");
        // DbContext has a Scoped service lifetime because:
        // 1. It ensures that a new instance of DbContext is created per request
        // 2. Db connections are a limited and expensive resource
        // 3. Dbcontext is not thread-safe. Scoped avoids to concureency issues
        // 4. makes it easier to manage transactions and ensure data consistency
        // 5. Reusing a DbContext instance can lead to increased memory usage

        builder.Services.AddNpgsql<GameStoreContext>(
                connString
        );
    }
}