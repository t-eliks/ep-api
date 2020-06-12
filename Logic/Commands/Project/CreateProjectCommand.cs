using DataAccess;
using DataAccess.Models;
using Logic.Base;
using Services.Interfaces.Project;

namespace Logic.Commands.Project
{
    public class CreateProjectCommand : BaseCommand<NewProjectViewModel, EmptyViewModel>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        public CreateProjectCommand(Repository repository, IProjectService projectService)
        {
            this.repository = repository;
            this.projectService = projectService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(NewProjectViewModel request)
        {
            projectService.CreateProject(CurrentApplicationUser, request);

            repository.SaveChanges();

            return GetGenericResponseSuccess(null);
        }
    }
}
