using DataAccess.Entities.User;
using DataAccess.Enums;
using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Assignment : PersistentEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? Deadline { get; set; }

        public AssignmentStatus Status { get; set; }

        public virtual Epic Epic { get; set; }

        public virtual ApplicationUser Assignee { get; set; }

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
