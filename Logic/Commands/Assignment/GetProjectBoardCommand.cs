using System.Linq;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Services.Interfaces.Assignment;
using Services.Interfaces.Project;

namespace Logic.Commands.Assignment
{
    public class GetProjectBoardCommand : BaseCommand<ProjectInfoRequestViewModel, BoardViewModel>
    {
        private readonly IProjectService projectService;

        private readonly IAssignmentService assignmentService;

        public GetProjectBoardCommand(IProjectService projectService, IAssignmentService assignmentService)
        {
            this.projectService = projectService;
            this.assignmentService = assignmentService;
        }

        protected override BaseResponse<BoardViewModel> ExecuteCore(ProjectInfoRequestViewModel request)
        {
            var validationResult = ValidateProjectAccess(projectService, request.ProjectId);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var assignments = assignmentService.GetAssignments(validationResult.Project);

            var assignmentDtos = assignments
                .Select(x => new BoardAssignmentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = x.Status,
                    AssigneeFirstName = x.Assignee?.FirstName,
                    AssigneeLastName = x.Assignee?.LastName,
                    CommentCount = x.Comments.Count(x => !x.DeletedOn.HasValue),
                    Deadline = x.Deadline,
                    CreatedOn = x.CreatedOn
                })
                .OrderBy(x => x.CreatedOn)
                .ToList();

            var viewModel = new BoardViewModel
            {
                Assignments = assignmentDtos
            };

            return GetResponseSuccess(viewModel);
        }
    }
}
