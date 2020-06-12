using DataAccess.Entities.User;
using System;

namespace DataAccess.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }

        DateTime CreatedOn { get; set; }

        ApplicationUser CreatedBy { get; set; }

        ApplicationUser ModifiedBy { get; set; }

        DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Is visible in queries
        /// </summary>
        bool Visible { get; }
    }
}
