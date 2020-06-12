using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Enums;
using DataAccess.Models;
using Logic.Base;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.User
{
    public class GetDashboardCommand : BaseCommand<DashboardViewModel>
    {
        private readonly Repository repository;

        public GetDashboardCommand(Repository repository)
        {
            this.repository = repository;
        }

        protected override BaseResponse<DashboardViewModel> ExecuteCore()
        {
            var dashboardProjectDtos = repository.ProjectToUser
                .Read(x => x.Collaborator == CurrentApplicationUser)
                .Include(x => x.Project)
                .ThenInclude(x => x.Epics)
                .ThenInclude(x => x.Assignments)
                .Where(x => !x.Project.DeletedOn.HasValue)
                .Select(x => new DashboardProjectDto
                {
                    Id = x.Project.Id,
                    Name = x.Project.Name,
                    Description = x.Project.Description,
                    CollaboratorCount = x.Project.Collaborators.Count(),
                    AssignmentCount = x.Project.Epics.SelectMany(x => x.Assignments).Count(),
                    IsCreator = x.Project.CreatedBy == CurrentApplicationUser,
                    CompletedCount = x.Project.Epics.SelectMany(x => x.Assignments).Where(x => x.Status == AssignmentStatus.Complete).Count(),
                    CreatedOn = x.CreatedOn
                })
                .ToList();

            var viewModel = new DashboardViewModel
            {
                ProjectDtos = dashboardProjectDtos
            };

            return GetResponseSuccess(viewModel);
        }
    }
}
