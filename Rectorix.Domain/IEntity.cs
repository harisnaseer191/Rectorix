using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Domain
{
    public interface IEntity
    {
        public int Id { get; set; }
    }

    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
