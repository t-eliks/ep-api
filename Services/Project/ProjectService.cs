using System.Linq;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Entities.Junctions;
using DataAccess.Entities.User;
using DataAccess.Models;
using Localization;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly Repository repository;


        public ProjectService(Repository repository)
        {
            this.repository = repository;
        }

        public DataAccess.Entities.Project GetProject(int projectId)
        {
            return repository.Projects
                .ReadNotDeleted(x => x.Id == projectId)
                .Include(x => x.Collaborators)
                .ThenInclude(x => x.Collaborator)
                .SingleOrDefault();
        }

        public (DataAccess.Entities.Project Project, string ErrorMessage, bool Unauthorized) ValidateAccess(ApplicationUser user, int projectId)
        {
            var project = GetProject(projectId);

            return ValidateAccess(user, project);
        }

        public (DataAccess.Entities.Project Project, string ErrorMessage, bool Unauthorized) ValidateAccess(ApplicationUser user, DataAccess.Entities.Project project)
        {
            if (project is null)
                return (null, Resources.Errors_ProjectDoesNotExist, false);

            if (!project.Collaborators.Select(x => x.Collaborator).Contains(user))
                return (null, Resources.Errors_Unauthorized, true);

            return (project, null, false);
        }

        public void CreateProject(ApplicationUser createdBy, NewProjectViewModel model)
        {
            var project = new DataAccess.Entities.Project
            {
                Name = model.Name,
                Description = model.Description
            };

            repository.Create(project, createdBy);

            repository.Create(new ProjectToUserJunction
            {
                Project = project,
                Collaborator = createdBy
            }, createdBy);
        }

        public void CreateEpic(ApplicationUser createdBy, DataAccess.Entities.Project project, NewEditEpicViewModel model)
        {
            var epic = new Epic()
            {
                Name = model.Name,
                Description = model.Description,
                Project = project
            };

            repository.Create(epic, createdBy);
        }

        public void DeleteEpic(Epic epic, ApplicationUser deletedBy)
        {
            repository.Delete(epic, deletedBy);

            epic.Assignments.ForEach(x =>
            {
                repository.Delete(x, deletedBy);
            });
        }
    }
}
