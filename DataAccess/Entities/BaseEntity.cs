using DataAccess.Entities.User;
using DataAccess.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class BaseEntity : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ApplicationUser ModifiedBy { get; set; }

        bool IEntity.Visible => true;
    }
}
