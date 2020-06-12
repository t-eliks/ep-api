using System.Collections.Generic;

namespace DataAccess.Dtos
{
    public class ProjectDetailsMemberDto
    {
        public int Index { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CompletedAssignments { get; set; }

        public IList<AssignmentDto> Assignments { get; set; }

        public bool IsCreator { get; set; }
    }
}
