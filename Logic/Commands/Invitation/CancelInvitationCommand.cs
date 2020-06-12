using System.Linq;
using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Logic.Commands.Invitation
{
    public class CancelInvitationCommand : BaseCommand<GenericRequestViewModel<int>, EmptyViewModel>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        public CancelInvitationCommand(Repository repository, IProjectService projectService)
        {
            this.repository = repository;
            this.projectService = projectService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(GenericRequestViewModel<int> request)
        {
            var invitation = repository
                .Invitations
                .Read(x => x.Id == request.Data)
                .Include(x => x.Project)
                .ThenInclude(x => x.Collaborators)
                .FirstOrDefault();

            var validationResult = ValidateProjectAccess(projectService, invitation.Project.Id);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            repository.Delete(invitation, CurrentApplicationUser);

            repository.SaveChanges();

            return GetResponseSuccess(message: Resources.Invitation_Canceled);
        }
    }
}
