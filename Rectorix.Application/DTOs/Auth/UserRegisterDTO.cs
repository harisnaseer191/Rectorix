using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Application.DTOs.Auth
{
    public class UserRegisterDto
    {
        // Basic Info
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Role { get; set; } = default!;

        // Address Info
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public string Country { get; set; } = default!;
    }

}
