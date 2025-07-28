using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rectorix.Domain.Entities;
using Rectorix.Persistence.Data;
using Rectorix.Persistence.DbContext;

namespace Rectorix.Persistence.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRoles>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<RectorixDBContext>();

            await AddPermissionsAsync(dbContext);
            await AddRolesAsync(roleManager, dbContext);
        }

        private static async Task AddPermissionsAsync(RectorixDBContext dbContext)
        {
            var permissions = new[]
            {
                new Permissions { Name = UserPermissions.ViewTenants, Description = "View tenants" },
                new Permissions { Name = UserPermissions.CreateTenants, Description = "Create tenants" },
                new Permissions { Name = UserPermissions.UpdateTenants, Description = "Update tenants" },
                new Permissions { Name = UserPermissions.DeleteTenants, Description = "Delete tenants" },
                // Add more permissions here
            };

            foreach (var permission in permissions)
            {
                if (!await dbContext.Permissions.AnyAsync(p => p.Name == permission.Name))
                {
                    dbContext.Permissions.Add(permission);
                }
            }
            await dbContext.SaveChangesAsync();
        }

        private static async Task AddRolesAsync(RoleManager<UserRoles> roleManager, RectorixDBContext dbContext)
        {
            var roles = new[] { RoleNames.AdminRole, RoleNames.PrincipalRole, RoleNames.TeacherRole, RoleNames.StudentRole };

            foreach (var roleName in roles)
            {
                if (!await roleManager.Roles.AnyAsync(r => r.Name == roleName))
                {
                    await roleManager.CreateAsync(new UserRoles { Name = roleName });
                }
            }

            // Assign all permissions to Admin
            var adminRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == RoleNames.AdminRole);
            if (adminRole != null)
            {
                var allPermissions = await dbContext.Permissions.ToListAsync();
                var existingPermissions = await dbContext.RolePermissions
                    .Where(rp => rp.RoleId == adminRole.Id)
                    .Select(rp => rp.PermissionId)
                    .ToListAsync();

                var newPermissions = allPermissions
                    .Where(p => !existingPermissions.Contains(p.Id))
                    .Select(p => new RolePermissions
                    {
                        RoleId = adminRole.Id,
                        PermissionId = p.Id
                    });

                if (newPermissions.Any())
                {
                    dbContext.RolePermissions.AddRange(newPermissions);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}