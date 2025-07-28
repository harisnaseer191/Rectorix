using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Domain.Entities
{
    public class Permissions : BaseEntity<long>
    {

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

    }
}
