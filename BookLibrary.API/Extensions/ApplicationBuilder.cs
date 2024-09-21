using BookLibrary.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.API.Extensions
{
    public static class ApplicationBuilder
    {
        public static void AddSeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            SeedData.Initialize(services);
        }

        public static void ApplyMigration(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using EFContext eFContext = scope.ServiceProvider.GetService<EFContext>();
            var pendingMigrations = eFContext.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                eFContext.Database.Migrate();
            }
            else
            {
                Console.WriteLine("No pending migrations.");
            }
        }
    }
}
