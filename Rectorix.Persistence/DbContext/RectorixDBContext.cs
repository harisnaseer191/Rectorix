using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rectorix.Domain.Entities;

namespace Rectorix.Persistence.DbContext
{
    public class RectorixDBContext: IdentityDbContext<ApplicationUser, UserRoles, long>
    {
        public RectorixDBContext(DbContextOptions<RectorixDBContext> options) : base(options) { }

        public DbSet<Permissions> Permissions => Set<Permissions>();
        public DbSet<RolePermissions> RolePermissions => Set<RolePermissions>();

        public DbSet<Address> Addresses {  get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
    }
}
