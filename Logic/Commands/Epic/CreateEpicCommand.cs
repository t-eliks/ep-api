using DataAccess;
using DataAccess.Models;
using Logic.Base;
using Services.Interfaces.Project;

namespace Logic.Commands.Epic
{
    public class CreateEpicCommand : BaseCommand<NewEditEpicViewModel, EmptyViewModel>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        public CreateEpicCommand(Repository repository, IProjectService projectService)
        {
            this.repository = repository;
            this.projectService = projectService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(NewEditEpicViewModel request)
        {
            var projectId = ParseIntParam(request.ProjectId);

            var validationResult = ValidateProjectAccess(projectService, projectId);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            projectService.CreateEpic(CurrentApplicationUser, validationResult.Project, request);

            repository.SaveChanges();

            return GetGenericResponseSuccess(null);
        }
    }
}
