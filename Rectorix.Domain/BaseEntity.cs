using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Domain
{
   
    public interface IAuditedEntity<TKey>: IEntity<TKey>
    {
        public DateTime CreateTime { get; set; }
        public TKey? CreatedByUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public TKey? ModifiedByUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public TKey? DeletedByUserId { get; set; }
        public bool IsDeleted { get; set; }


    }

    public abstract class BaseEntity<TKey> : IAuditedEntity<TKey>
    {
        public DateTime CreateTime { get; set; }
        public TKey? CreatedByUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public TKey? ModifiedByUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public TKey? DeletedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public TKey Id { get; set; } = default!;
    }
}
