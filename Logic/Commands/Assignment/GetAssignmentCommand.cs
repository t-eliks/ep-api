using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.Assignment;
using Services.Interfaces.Project;

namespace Logic.Commands.Assignment
{
    public class GetAssignmentCommand : BaseCommand<AssignmentInfoRequestViewModel, AssignmentViewModel>
    {
        private readonly IProjectService projectService;

        private readonly IAssignmentService assignmentService;

        public GetAssignmentCommand(IProjectService projectService, IAssignmentService assignmentService)
        {
            this.projectService = projectService;
            this.assignmentService = assignmentService;
        }

        protected override BaseResponse<AssignmentViewModel> ExecuteCore(AssignmentInfoRequestViewModel request)
        {
            var assignment = assignmentService.GetAssignment(request.AssignmentId);

            if (assignment is null)
                return GetResponseFailed(Resources.Errors_AssignmentDoesNotExist);

            var validationResult = ValidateProjectAccess(projectService, assignment.Epic.Project.Id);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var assignmentViewModel = new AssignmentViewModel
            {
                Id = assignment.Id,
                Name = assignment.Name,
                Status = assignment.Status,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                AssigneeId = assignment.Assignee?.Id.ToString(),
                CreatedOn = assignment.CreatedOn,
                AuthorFirstName = assignment.CreatedBy.FirstName,
                AuthorLastName = assignment.CreatedBy.LastName,
                ModifiedOn = assignment.ModifiedOn,
                LastModifiedFirstName = assignment.ModifiedBy?.FirstName,
                LastModifiedLastName = assignment.ModifiedBy?.LastName
            };

            return GetResponseSuccess(assignmentViewModel);
        }
    }
}
