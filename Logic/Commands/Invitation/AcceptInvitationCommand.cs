using System.Linq;
using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.Invitation;

namespace Logic.Commands.Invitation
{
    public class AcceptInvitationCommand : BaseCommand<PendingInvitationRequestViewModel, EmptyViewModel>
    {
        private readonly Repository repository;

        private readonly IInvitationService invitationService;

        public override bool AllowAnonymous => true;

        public AcceptInvitationCommand(Repository repository, IInvitationService invitationService)
        {
            this.repository = repository;
            this.invitationService = invitationService;
        }
        protected override BaseResponse<EmptyViewModel> ExecuteCore(PendingInvitationRequestViewModel request)
        {
            var pendingInvitation = invitationService.GetInvitation(x => x.Token == request.Token);

            if (pendingInvitation is null)
                return GetResponseFailed(Resources.Errors_InvitationInvalid).WithStatusCode(ResponseStatusCodes.Ok);

            if (pendingInvitation.Project.Collaborators.Select(x => x.Collaborator.Email).Contains(CurrentApplicationUser.Email))
                return GetResponseFailed(Resources.Errors_CollaboratorAlreadyExists).WithStatusCode(ResponseStatusCodes.Ok);

            if (pendingInvitation.Email != CurrentApplicationUser.Email)
                return GetResponseFailed(Resources.Errors_InvitationUnauthorized).WithStatusCode(ResponseStatusCodes.Ok);

            invitationService.AcceptInvitation(CurrentApplicationUser, pendingInvitation);

            repository.SaveChanges();

            return GetResponseSuccess(message: Resources.Invitation_Accepted);
        }
    }
}
