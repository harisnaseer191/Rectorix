// Persistence/Seeders/TenantCatalogSeeder.cs
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rectorix.Domain.DomainShared;
using Rectorix.Persistence.DbContext;          // TenantCatalogDbContext

namespace Rectorix.Persistence.Seeders
{


    public static class TenantCatalogSeeder
    {
        /// <summary>Ensures the catalog database exists, is migrated, and contains baseline tenants.</summary>
        public static async Task SeedAsync(IServiceProvider root)
        {
            await using var scope = root.CreateAsyncScope();

            // 1️⃣  Apply any pending migrations (safe on every startup)
            var catalogDb = scope.ServiceProvider.GetRequiredService<TenantCatalogDbContext>();
            await catalogDb.Database.MigrateAsync();

            // 2️⃣  Access the Finbuckle store interface (works the same no matter which store you use)
            var store = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<RectorixTenantInfo>>();

            // 3️⃣  Add demo tenants only if they don't already exist
            var existing = (await store.GetAllAsync()).Select(t => t.Identifier).ToHashSet();

            var demoTenants = new[]
            {
            new RectorixTenantInfo
            {
                Id         = "1",
                Identifier = "alpha",              // → alpha.your-host.com
                Name       = "Alpha School"
            },
            new RectorixTenantInfo
            {
                Id         = "2",
                Identifier = "beta",
                Name       = "Beta Academy"
            }
        };

            foreach (var t in demoTenants.Where(t => !existing.Contains(t.Identifier)))
                await store.TryAddAsync(t);            // Finbuckle handles concurrency & caching
        }
    }

}
