using System.Linq;
using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Logic.Commands.Epic
{
    public class UpdateEpicCommand : BaseCommand<EditEpicRequestViewModel, EmptyViewModel>
    {
        private readonly Repository repository;
        
        private readonly IProjectService projectService;
        
        public UpdateEpicCommand(IProjectService projectService, Repository repository)
        {
            this.projectService = projectService;
            this.repository = repository;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(EditEpicRequestViewModel request)
        {
            var epic = repository
                .Epics
                .ReadNotDeleted(x => x.Id == request.EpicId)
                .Include(x => x.Project)
                .SingleOrDefault();

            var validationResult = ValidateProjectAccess(projectService, epic.Project.Id);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }
            
            var viewModel = request.ViewModel;

            epic.Name = viewModel.Name;
            epic.Description = viewModel.Description;

            repository.Update(epic, CurrentApplicationUser);
            repository.SaveChanges();
            
            return GetResponseSuccess(message: Resources.EpicUpdatedSuccessfully);
        }
    }
}