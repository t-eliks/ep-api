using System.Linq;
using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Logic.Commands.Epic
{
    public class DeleteEpicCommand : BaseCommand<GenericRequestViewModel<int>, EmptyViewModel>
    {
        private readonly Repository repository;
        
        private readonly IProjectService projectService;
        
        public DeleteEpicCommand(IProjectService projectService, Repository repository)
        {
            this.projectService = projectService;
            this.repository = repository;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(GenericRequestViewModel<int> request)
        {
            var epic = repository
                .Epics
                .ReadNotDeleted(x => x.Id == request.Data)
                .Include(x => x.Project)
                .Include(x => x.Assignments)
                .SingleOrDefault();

            if (epic is null)
            {
                return GetGenericResponseFailed();
            }
            
            var validationResult = ValidateProjectAccess(projectService, epic.Project.Id);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }

            projectService.DeleteEpic(epic, CurrentApplicationUser);
            repository.SaveChanges();
            
            return GetResponseSuccess(message: Resources.EpicDeletedSuccessfully);
        }
    }
}