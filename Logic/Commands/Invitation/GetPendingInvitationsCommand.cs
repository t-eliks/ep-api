using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Logic.Commands.Invitation
{
    public class GetPendingInvitationsCommand : BaseCommand<GenericProjectRequestViewModel, IList<PendingInvitationDto>>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        public GetPendingInvitationsCommand(Repository repository, IProjectService projectService)
        {
            this.repository = repository;
            this.projectService = projectService;
        }

        protected override BaseResponse<IList<PendingInvitationDto>> ExecuteCore(GenericProjectRequestViewModel request)
        {
            var validationResult = ValidateProjectAccess(projectService, request.ProjectId);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var project = validationResult.Project;

            var pendingInvitations = repository.Invitations
                .Read(x => x.Project == project)
                .Include(x => x.CreatedBy)
                .Select(x => new PendingInvitationDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    CreatedOn = x.CreatedOn,
                    InvitedByFirstName = x.CreatedBy.FirstName,
                    InvitedByLastName = x.CreatedBy.LastName
                })
                .ToList();
            
            return GetResponseSuccess(pendingInvitations);
        }
    }
}
