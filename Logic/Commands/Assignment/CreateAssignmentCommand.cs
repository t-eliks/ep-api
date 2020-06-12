using System.Linq;
using DataAccess;
using DataAccess.Entities.User;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Assignment;
using Services.Interfaces.Project;
using Services.Interfaces.User;

namespace Logic.Commands.Assignment
{
    public class CreateAssignmentCommand : BaseCommand<NewAssignmentViewModel, EmptyViewModel>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        private readonly IAssignmentService assignmentService;

        private readonly IUserService userService;

        public CreateAssignmentCommand(Repository repository, IProjectService projectService, IAssignmentService assignmentService, 
            IUserService userService)
        {
            this.repository = repository;
            this.projectService = projectService;
            this.assignmentService = assignmentService;
            this.userService = userService;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore(NewAssignmentViewModel request)
        {
            var epicId = ParseIntParam(request.EpicId);

            var epic = repository.Epics
                .ReadNotDeleted(x => x.Id == epicId)
                .Include(x => x.Project)
                .ThenInclude(x => x.Collaborators)
                .SingleOrDefault();

            if (epic is null)
                return GetResponseFailed(Resources.Errors_EpicDoesNotExist);

            var validationResult = ValidateProjectAccess(projectService, epic.Project.Id);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var project = validationResult.Project;

            ApplicationUser user = null;

            if (request.AssigneeId.HasValue)
            {
                var assigneeId = request.AssigneeId;

                user = userService.GetById(assigneeId.Value);

                if (user is null || !project.Collaborators.Select(x => x.Collaborator).Contains(user))
                    return GetResponseFailed(Resources.Errors_Unauthorized);
            }

            assignmentService.CreateAssignment(CurrentApplicationUser, user, epic, request.Deadline, request.Name, request.Description);

            repository.SaveChanges();

            return GetResponseSuccess(message: Resources.Assignment_Created);
        }
    }
}
