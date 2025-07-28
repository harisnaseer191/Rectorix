using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Persistence.Data
{
    public class UserPermissions
    {
        public const string ViewTenants = "Permissions.Tenants.View";
        public const string CreateTenants = "Permissions.Tenants.Create";
        public const string UpdateTenants = "Permissions.Tenants.Update";
        public const string DeleteTenants = "Permissions.Tenants.Delete";
        // Add more permission constants here
    }
}
