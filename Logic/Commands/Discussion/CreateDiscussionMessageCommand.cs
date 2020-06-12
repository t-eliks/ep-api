using System;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models;
using Logic.Base;
using Logic.Hubs;
using Microsoft.AspNetCore.SignalR;
using Services.Interfaces.Project;

namespace Logic.Commands.Discussion
{
    public class CreateDiscussionMessageCommand : BaseCommand<CreateDiscussionMessageRequestViewModel, EmptyViewModel>
    {
        private readonly IProjectService projectService;

        private readonly Repository repository;

        private readonly IHubContext<DiscussionHub> discussionHub;

        public CreateDiscussionMessageCommand(IProjectService projectService, Repository repository, IHubContext<DiscussionHub> discussionHub)
        {
            this.projectService = projectService;
            this.repository = repository;
            this.discussionHub = discussionHub;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(CreateDiscussionMessageRequestViewModel request)
        {
            var validationResult = ValidateProjectAccess(projectService, request.ProjectId);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }

            var project = validationResult.Project;

            var discussionMessage = new DiscussionMessage
            {
                CreatedBy = CurrentApplicationUser,
                CreatedOn = DateTime.UtcNow,
                Project = project,
                Content = request.Content
            };

            repository.Create(discussionMessage, CurrentApplicationUser);

            repository.SaveChanges();

            discussionHub.Clients.Group(project.Id.ToString())
                .SendAsync("NewDiscussionMessage", discussionMessage.Id.ToString());

            return GetGenericResponseSuccess(null);
        }
    }
}
