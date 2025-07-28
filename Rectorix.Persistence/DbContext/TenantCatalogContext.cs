// Persistence/TenantCatalogDbContext.cs
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.EntityFrameworkCore;
using Rectorix.Domain.DomainShared;

namespace Rectorix.Persistence.DbContext
{
  

    public class TenantCatalogDbContext
           : EFCoreStoreDbContext<RectorixTenantInfo>
    {
        public TenantCatalogDbContext(DbContextOptions<TenantCatalogDbContext> opt)
            : base(opt) { }

        // optional mapping tweaks
        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.Entity<RectorixTenantInfo>().ToTable("Tenants", "catalog");
        }
    }

}
