using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace NoName.Data
{
    public static class DbContextExtension
    {
        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>().GetAppliedMigrations().Select(m => m.MigrationId);
            if (applied.Count() == 0)
            {
                return false;
            }
            var total = context.GetService<IMigrationsAssembly>().Migrations.Select(m => m.Key);
            return !total.Except(applied).Any();
        }
    }
}