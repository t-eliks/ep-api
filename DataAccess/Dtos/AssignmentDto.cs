using DataAccess.Enums;
using System;
using System.Collections.Generic;

namespace DataAccess.Dtos
{
    public class AssignmentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AssignmentStatus Status { get; set; }

        public DateTime? Deadline { get; set; }

        public string AssigneeFirstName { get; set; }

        public string AssigneeLastName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
