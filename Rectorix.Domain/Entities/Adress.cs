using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Domain.Entities
{
    public class Address: BaseEntity<long>
    {
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public string Country { get; set; } = default!;

        public long UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;
    }

}
