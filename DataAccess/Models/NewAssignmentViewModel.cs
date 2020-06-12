using System;

namespace DataAccess.Models
{
    public class NewAssignmentViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string EpicId { get; set; }

        public DateTime? Deadline { get; set; }

        public int? AssigneeId { get; set; }
    }
}
