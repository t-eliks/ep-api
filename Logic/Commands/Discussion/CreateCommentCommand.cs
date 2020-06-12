using System;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.Assignment;
using Services.Interfaces.Notification;
using Services.Interfaces.Project;

namespace Logic.Commands.Discussion
{
    public class CreateCommentCommand : BaseCommand<PostCommentRequestViewModel, EmptyViewModel>
    {
        private readonly Repository repository;
        private readonly IAssignmentService assignmentService;
        private readonly IProjectService projectService;
        private readonly INotificationService notificationService;

        public CreateCommentCommand(Repository repository, IAssignmentService assignmentService,
            IProjectService projectService, INotificationService notificationService)
        {
            this.repository = repository;
            this.assignmentService = assignmentService;
            this.projectService = projectService;
            this.notificationService = notificationService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(PostCommentRequestViewModel request)
        {
            var assignment = assignmentService.GetAssignment(request.AssignmentId);

            if (assignment is null)
                return GetResponseFailed(Resources.Errors_AssignmentDoesNotExist);

            var validationResult = ValidateProjectAccess(projectService, assignment.Epic.Project.Id);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var comment = new Comment
            {
                Assignment = assignment,
                CreatedBy = CurrentApplicationUser,
                CreatedOn = DateTime.UtcNow,
                Content = request.Content,
            };

            repository.Add(comment);

            if (assignment.Assignee != null)
            {
                notificationService.CreateNewCommentNotification(CurrentApplicationUser, assignment.Assignee, assignment);
            }

            repository.SaveChanges();

            return GetGenericResponseSuccess(null).WithStatusCode(ResponseStatusCodes.Created);
        }
    }
}