using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Assignment;
using Services.Interfaces.Project;

namespace Logic.Commands.Epic
{
    public class GetProjectBacklogCommand : BaseCommand<ProjectInfoRequestViewModel, BacklogViewModel>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        private readonly IAssignmentService assignmentService;

        public GetProjectBacklogCommand(Repository repository, IProjectService projectService, IAssignmentService assignmentService)
        {
            this.repository = repository;
            this.projectService = projectService;
            this.assignmentService = assignmentService;
        }

        protected override BaseResponse<BacklogViewModel> ExecuteCore(ProjectInfoRequestViewModel request)
        {
            var validationResult = ValidateProjectAccess(projectService, request.ProjectId);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var epicDtos = repository.Epics.ReadNotDeleted(x => x.Project == validationResult.Project)
                .Include(x => x.Assignments)
                .Select(x => new EpicBacklogDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Assignments = x.Assignments
                        .Where(x => !x.DeletedOn.HasValue)
                        .Select(a => assignmentService.GetAssignmentDto(a)).ToList()
                }).ToList();

            var upcomingAssigment = epicDtos
                .SelectMany(x => x.Assignments)
                .OrderBy(x => x.Deadline)
                .FirstOrDefault();

            var viewModel = new BacklogViewModel
            {
                Epics = epicDtos,
                EarliestDeadline = upcomingAssigment?.Deadline,
                EarliestDeadlineAssignmentId = upcomingAssigment?.Id
            };

            return GetResponseSuccess(viewModel);
        }
    }
}
