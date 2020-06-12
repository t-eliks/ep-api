using System.Linq;
using DataAccess;
using DataAccess.Entities.User;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Notification;
using Services.Interfaces.Project;

namespace Logic.Commands.Assignment
{
    public class UpdateAssignmentCommand : BaseCommand<EditAssignmentRequestViewModel, EmptyViewModel>
    {
        private readonly Repository repository;
        
        private readonly IProjectService projectService;

        private readonly INotificationService notificationService;
        
        public UpdateAssignmentCommand(IProjectService projectService, Repository repository, INotificationService notificationService)
        {
            this.projectService = projectService;
            this.repository = repository;
            this.notificationService = notificationService;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(EditAssignmentRequestViewModel request)
        {
            var assignment = repository
                .Assignments
                .ReadNotDeleted(x => x.Id == request.AssignmentId)
                .Include(x => x.Assignee)
                .Include(x => x.Epic)
                .ThenInclude(x => x.Project)
                .SingleOrDefault();

            var validationResult = ValidateProjectAccess(projectService, assignment.Epic.Project.Id);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }
            
            var viewModel = request.ViewModel;

            ApplicationUser assignee = null;
            
            if (viewModel.AssigneeId != -1)
            {
                assignee = repository
                    .Users
                    .SingleOrDefault(x => x.Id == viewModel.AssigneeId);

                if (assignee == null)
                {
                    return GetGenericResponseFailed();
                }
            }
            
            if (viewModel.AssigneeId.HasValue && 
                viewModel.AssigneeId > 0 &&
                viewModel.AssigneeId != CurrentApplicationUser.Id)
            {
                if (assignment.Assignee is null || assignment.Assignee.Id != viewModel.AssigneeId)
                {
                    notificationService.CreateAssignedToTaskNotification(CurrentApplicationUser, assignee, assignment);
                }
                else
                {
                    notificationService.CreateAssignmentUpdatedNotification(CurrentApplicationUser, assignee, assignment);
                }
            }
            
            assignment.Name = viewModel.Name;
            assignment.Description = viewModel.Description;
            assignment.Deadline = viewModel.Deadline;
            assignment.Assignee = assignee;

            repository.Update(assignment, CurrentApplicationUser);

            repository.SaveChanges();
            
            return GetResponseSuccess(message: Resources.AssignmentUpdatedSuccessfully);
        }
    }
}