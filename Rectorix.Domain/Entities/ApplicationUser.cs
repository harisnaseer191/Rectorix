using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Rectorix.Domain.Entities
{
    public class ApplicationUser: IdentityUser<long>
    {
        public string FullName { get; set; } = default!;
        public string? ProfilePictureUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public virtual Address? Address { get; set; }


    }
}
