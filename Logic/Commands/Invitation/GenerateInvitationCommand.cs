using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;
using System;
using System.Linq;

namespace Logic.Commands.Invitation
{
    public class GenerateInvitationCommand : BaseCommand<InvitationRequestViewModel, string>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        public GenerateInvitationCommand(Repository repository, IProjectService projectService)
        {
            this.repository = repository;
            this.projectService = projectService;
        }

        protected override BaseResponse<string> ExecuteCore(InvitationRequestViewModel request)
        {
            var validationResult = ValidateProjectAccess(projectService, request.ProjectId);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var project = validationResult.Project;

            if (project.Collaborators.Select(x => x.Collaborator.Email).Contains(request.Email))
                return GetResponseFailed(Resources.Errors_CollaboratorAlreadyExists).WithStatusCode(ResponseStatusCodes.Ok);

            var pendingEmails = repository.Invitations
                .Read(x => x.Project == project)
                .Include(x => x.Project).Select(x => x.Email).ToList();

            if (pendingEmails.Contains(request.Email))
                return GetResponseFailed(Resources.Errors_CollaboratorAlreadyPending).WithStatusCode(ResponseStatusCodes.Ok);

            var token = Guid.NewGuid().ToString();

            var invitation = new DataAccess.Entities.Invitation
            {
                Project = project,
                Token = token,
                Email = request.Email
            };

            repository.Create(invitation, CurrentApplicationUser);

            repository.SaveChanges();

            var link = $"{request.OriginEndpoint}/{token}";

            return GetResponseSuccess(link);
        }
    }
}
