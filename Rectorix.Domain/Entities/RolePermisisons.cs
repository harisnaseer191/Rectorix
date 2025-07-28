using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Domain.Entities
{
    public class RolePermissions: BaseEntity<long>
    {
        public long RoleId { get; set; } = default!;
        public long PermissionId { get; set; }

        public UserRoles Role { get; set; } = default!;
        public Permissions Permission { get; set; } = default!;
    }

}
