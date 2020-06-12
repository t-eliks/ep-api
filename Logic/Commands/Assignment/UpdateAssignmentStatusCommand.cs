using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.Assignment;
using Services.Interfaces.Project;

namespace Logic.Commands.Assignment
{
    public class UpdateAssignmentStatusCommand : BaseCommand<UpdateAssignmentStatusRequestViewModel, EmptyViewModel>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        private readonly IAssignmentService assignmentService;

        public UpdateAssignmentStatusCommand(Repository repository, IProjectService projectService, IAssignmentService assignmentService)
        {
            this.repository = repository;
            this.projectService = projectService;
            this.assignmentService = assignmentService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(UpdateAssignmentStatusRequestViewModel request)
        {
            var assignment = assignmentService.GetAssignment(request.AssignmentId);

            if (assignment is null)
                return GetResponseFailed(Resources.Errors_AssignmentDoesNotExist);

            var validationResult = ValidateProjectAccess(projectService, assignment.Epic.Project.Id);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            assignment.Status = request.Status;

            repository.Update(assignment, CurrentApplicationUser);

            repository.SaveChanges();

            return GetResponseSuccess();
        }
    }
}
