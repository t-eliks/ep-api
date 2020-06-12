using DataAccess.Entities.User;
using System;

namespace DataAccess.Interfaces
{
    public interface IPersistentEntity : IEntity
    {
        DateTime? DeletedOn { get; set; }

        ApplicationUser DeletedBy { get; set; }
    }
}
