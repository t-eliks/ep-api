using DataAccess.Enums;

namespace DataAccess.Models
{
    public class UpdateAssignmentStatusRequestViewModel
    {
        public int AssignmentId { get; set; }

        public AssignmentStatus Status { get; set; }

        public int? AssignmentBeforeId { get; set; }
    }
}
