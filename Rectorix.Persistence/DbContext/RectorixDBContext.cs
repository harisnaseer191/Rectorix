using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rectorix.Domain;
using Rectorix.Domain.DomainShared;
using Rectorix.Domain.Entities;
using System;

namespace Rectorix.Persistence.DbContext
{
    public class RectorixDBContext: IdentityDbContext<ApplicationUser, UserRoles, long>
    {
        private readonly ITenantAccessor _tenant;

        public RectorixDBContext(DbContextOptions<RectorixDBContext> options,ITenantAccessor tenant) : base(options)
        {
            _tenant = tenant;
        }

        public DbSet<Permissions> Permissions => Set<Permissions>();
        public DbSet<RolePermissions> RolePermissions => Set<RolePermissions>();

        public DbSet<Address> Addresses {  get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Apply global filter to every entity that implements IMustHaveTenant
            foreach (var e in builder.Model.GetEntityTypes()
                                     .Where(t => typeof(IMustHaveTenant).IsAssignableFrom(t.ClrType)))
            {
                var method = typeof(RectorixDBContext).GetMethod(nameof(SetTenantFilter),
                             System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                             .MakeGenericMethod(e.ClrType);

                method.Invoke(null, new object?[] { builder, this });
            }

            builder.Entity<RolePermissions>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermissions>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermissions>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);



            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(255);
                entity.Property(e => e.DateOfBirth).HasColumnType("date");
                entity.HasOne(u => u.Address)
                        .WithOne(a => a.User)
                        .HasForeignKey<Address>(a => a.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            });
        }

        private static void SetTenantFilter<TEntity>(ModelBuilder builder, RectorixDBContext ctx)
        where TEntity : class, IMustHaveTenant
        => builder.Entity<TEntity>()
                    .HasQueryFilter(e => e.TenantId == (ctx._tenant.Current != null ? ctx._tenant.Current.Id : string.Empty));
    }
}
