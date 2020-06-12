using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Enums;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Invitation
{
    public class GetPendingInvitationInfoCommand : BaseCommand<PendingInvitationRequestViewModel, PendingInvitationDto>
    {
        private readonly Repository repository;

        public override bool AllowAnonymous => true;

        public GetPendingInvitationInfoCommand(Repository repository)
        {
            this.repository = repository;
        }
        protected override BaseResponse<PendingInvitationDto> ExecuteCore(PendingInvitationRequestViewModel request)
        {
            var pendingInvitation = repository.Invitations
                .Read(x => x.Token == request.Token)
                .Include(x => x.CreatedBy)
                .Include(x => x.Project)
                .Select(x => new PendingInvitationDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    CreatedOn = x.CreatedOn,
                    InvitedByFirstName = x.CreatedBy.FirstName,
                    InvitedByLastName = x.CreatedBy.LastName,
                    ProjectName = x.Project.Name,
                    AccountExists = repository.Users.Any(u => x.Email == u.Email)
                })
                .SingleOrDefault();

            if (pendingInvitation is null)
                return GetResponseFailed(Resources.Errors_InvitationInvalid).WithStatusCode(ResponseStatusCodes.Ok);

            if (CurrentApplicationUser != null && pendingInvitation.Email != CurrentApplicationUser.Email)
                return GetResponseFailed(Resources.Errors_InvitationUnauthorized).WithStatusCode(ResponseStatusCodes.Ok);


            return GetResponseSuccess(pendingInvitation);
        }
    }
}
