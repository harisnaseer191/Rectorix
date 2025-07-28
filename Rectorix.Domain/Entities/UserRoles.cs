using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Domain.Entities
{
    public class UserRoles : IdentityRole<long>
    {

        public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();

    }
}
