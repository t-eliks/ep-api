using System;

namespace DataAccess.Dtos
{
    public class DashboardProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CollaboratorCount { get; set; }

        public int AssignmentCount { get; set; }

        public bool IsCreator { get; set; }

        public int CompletedCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
