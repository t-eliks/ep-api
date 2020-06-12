using DataAccess.Entities.User;

namespace DataAccess.Entities.Junctions
{
    public class ProjectToUserJunction : BaseEntity
    {
        public Project Project { get; set; }

        public ApplicationUser Collaborator { get; set; }
    }
}
