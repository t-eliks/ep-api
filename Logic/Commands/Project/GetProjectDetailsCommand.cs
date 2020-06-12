using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Enums;
using DataAccess.Models;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Assignment;
using Services.Interfaces.Project;

namespace Logic.Commands.Project
{
    public class GetProjectDetailsCommand : BaseCommand<ProjectInfoRequestViewModel, ProjectDetailsViewModel>
    {
        private readonly Repository repository;

        private readonly IProjectService projectService;

        private readonly IAssignmentService assignmentService;

        public GetProjectDetailsCommand(Repository repository, IProjectService projectService, IAssignmentService assignmentService)
        {
            this.repository = repository;
            this.projectService = projectService;
            this.assignmentService = assignmentService;
        }

        protected override BaseResponse<ProjectDetailsViewModel> ExecuteCore(ProjectInfoRequestViewModel request)
        {
            var validationResult = ValidateProjectAccess(projectService, request.ProjectId);

            if (!validationResult.Response.Success)
                return validationResult.Response;

            var project = validationResult.Project;

            var assignments = repository.Assignments
                .ReadNotDeleted()
                .Include(x => x.Comments)
                .Include(x => x.Epic)
                .ThenInclude(x => x.Project)
                .Where(x => x.Epic.Project == project)
                .ToList();

            var completedAssignmentCount = assignments.Count(x => x.Status == AssignmentStatus.Complete);
            var leftTodoAssignmentCount = assignments.Count(x => x.Status != AssignmentStatus.Complete);

            var mostRecentAssignments = assignments
                .OrderBy(x => x.CreatedOn)
                .Take(3)
                .Select(x => assignmentService.GetAssignmentDto(x))
                .ToList();

            var members = project.Collaborators
                .Select(x => x.Collaborator)
                .OrderByDescending(x => project.CreatedBy == x)
                .ThenBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var memberDtos = new List<ProjectDetailsMemberDto>();

            var index = 0;

            members.ForEach(x =>
            {
                var memberAssignments = assignments.Where(a => a.Assignee == x);
                memberDtos.Add(new ProjectDetailsMemberDto
                {
                    Index = index++,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CompletedAssignments = memberAssignments.Count(x => x.Status == AssignmentStatus.Complete),
                    Assignments = memberAssignments
                        .Where(x => x.Status != AssignmentStatus.Complete)
                        .Select(x => assignmentService.GetAssignmentDto(x))
                        .ToList(),
                    IsCreator = project.CreatedBy == x
                });
            });

            var hasUnreadNotifications = repository.Notifications
                .Read(x => x.Project == project && 
                           x.User == CurrentApplicationUser && 
                           !x.IsRead)
                .Any();
            
            return GetResponseSuccess(new ProjectDetailsViewModel
            {
                Id = project.Id,
                IsCreator = project.CreatedBy == CurrentApplicationUser,
                Name = project.Name,
                Description = project.Description,
                CompletedAssignments = completedAssignmentCount,
                LeftToDo = leftTodoAssignmentCount,
                MemberAssignmentInfo = memberDtos,
                LatestAssignments = mostRecentAssignments,
                HasUnreadNotifications = hasUnreadNotifications
            });
        }
    }
}
