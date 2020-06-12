using DataAccess.Entities;
using DataAccess.Entities.User;
using DataAccess.Models;

namespace Services.Interfaces.Project
{
    public interface IProjectService
    {
        DataAccess.Entities.Project GetProject(int projectId);

        (DataAccess.Entities.Project Project, string ErrorMessage, bool Unauthorized) ValidateAccess(ApplicationUser user, int projectId);

        (DataAccess.Entities.Project Project, string ErrorMessage, bool Unauthorized) ValidateAccess(ApplicationUser user, DataAccess.Entities.Project project);

        void CreateProject(ApplicationUser createdBy, NewProjectViewModel model);

        void CreateEpic(ApplicationUser createdBy, DataAccess.Entities.Project project, NewEditEpicViewModel model);

        void DeleteEpic(Epic epic, ApplicationUser deletedBy);
    }
}
