using DataAccess.Entities.User;
using DataAccess.Interfaces;
using System;

namespace DataAccess.Entities
{
    public class PersistentEntity : BaseEntity, IPersistentEntity
    {
        public DateTime? DeletedOn { get; set; }

        public virtual ApplicationUser DeletedBy { get; set; }

        bool IEntity.Visible => DeletedOn.HasValue;
    }
}
