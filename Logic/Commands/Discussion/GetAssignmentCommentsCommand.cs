using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Assignment;
using Services.Interfaces.Project;

namespace Logic.Commands.Discussion
{
    public class GetAssignmentCommentsCommand : BaseCommand<AssignmentInfoRequestViewModel, IList<CommentDto>>
    {
        private readonly Repository repository;
        private readonly IAssignmentService assignmentService;
        private readonly IProjectService projectService;

        public GetAssignmentCommentsCommand(Repository repository, IAssignmentService assignmentService, IProjectService projectService)
        {
            this.repository = repository;
            this.assignmentService = assignmentService;
            this.projectService = projectService;
        }

        protected override BaseResponse<IList<CommentDto>> ExecuteCore(AssignmentInfoRequestViewModel request)
        {
            var assignment = assignmentService.GetAssignment(request.AssignmentId);

            if (assignment is null)
                return GetResponseFailed(Resources.Errors_AssignmentDoesNotExist);

            var validationResult = ValidateProjectAccess(projectService, assignment.Epic.Project.Id);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var comments = repository.Comments
                .ReadNotDeleted(x => x.Assignment == assignment)
                .Include(x => x.CreatedBy)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Assignee)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new CommentDto
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    Content = x.Content,
                    AssignmentId = x.Assignment.Id,
                    IsAssignee = x.CreatedBy == x.Assignment.Assignee,
                    AuthorId = x.CreatedBy.Id,
                    AuthorName = string.Join(' ', x.CreatedBy.FirstName, x.CreatedBy.LastName),
                    IsAuthor = x.CreatedBy == CurrentApplicationUser
                })
                .ToList();

            return GetResponseSuccess(comments);
        }
    }
}
