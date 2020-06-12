using System.Collections.Generic;

namespace DataAccess.Dtos
{
    public class EpicBacklogDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<AssignmentDto> Assignments { get; set; }
    }
}
