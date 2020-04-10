using GeographySample.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace GeographySample.WebApi
{
    public static class DbMigrationExtensions
    {
        public static IHost MigrateDb(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<GeographyDbContext>();
                try
                {
                    var applied = dbContext.GetService<IHistoryRepository>()
                        .GetAppliedMigrations()
                        .Select(m => m.MigrationId);

                    var total = dbContext.GetService<IMigrationsAssembly>()
                        .Migrations.Select(m => m.Key);

                    var hasPendingMigration = total.Except(applied).Any();
                    if (hasPendingMigration)
                        dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

            return host;
        }
    }
}