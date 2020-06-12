using System.Linq;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Services.Interfaces.Project;

namespace Logic.Commands.Invitation
{
    public class GetCollaboratorOverviewCommand : BaseCommand<CollaboratorOverviewRequestViewModel, CollaboratorOverviewViewModel>
    {
        private readonly IProjectService projectService;

        public GetCollaboratorOverviewCommand(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        protected override BaseResponse<CollaboratorOverviewViewModel> ExecuteCore(CollaboratorOverviewRequestViewModel request)
        {
            var projectId = ParseIntParam(request.ProjectId);

            var validationResult = ValidateProjectAccess(projectService, projectId);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var collaboratorOverviewDtos = validationResult.Project.Collaborators.Select(x => new CollaboratorOverview
            {
                Id = x.Collaborator.Id,
                FirstName = x.Collaborator.FirstName,
                LastName = x.Collaborator.LastName
            }).ToList();

            var response = new CollaboratorOverviewViewModel { Collaborators = collaboratorOverviewDtos };

            return GetResponseSuccess(response);
        }
    }
}
