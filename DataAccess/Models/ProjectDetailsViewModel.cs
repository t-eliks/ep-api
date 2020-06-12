using DataAccess.Dtos;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }

        public bool IsCreator { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int CompletedAssignments { get; set; }

        public int LeftToDo { get; set; }

        public bool HasUnreadNotifications { get; set; }
        
        public IList<AssignmentDto> LatestAssignments { get; set; }

        public IList<ProjectDetailsMemberDto> MemberAssignmentInfo { get; set; }
    }
}
