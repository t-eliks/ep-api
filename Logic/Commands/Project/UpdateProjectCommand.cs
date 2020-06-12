using System.Linq;
using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.Project;

namespace Logic.Commands.Project
{
    public class UpdateProjectCommand : BaseCommand<EditProjectRequestViewModel, EmptyViewModel>
    {
        private readonly Repository repository;
        
        private readonly IProjectService projectService;
        
        public UpdateProjectCommand(IProjectService projectService, Repository repository)
        {
            this.projectService = projectService;
            this.repository = repository;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(EditProjectRequestViewModel request)
        {
            var project = repository
                .Projects
                .ReadNotDeleted(x => x.Id == request.ProjectId)
                .SingleOrDefault();

            var validationResult = ValidateProjectAccess(projectService, project.Id);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }
            
            if (project.CreatedBy != CurrentApplicationUser)
            {
                return GetGenericResponseFailed();
            }
            
            var viewModel = request.ViewModel;

            project.Name = viewModel.Name;
            project.Description = viewModel.Description;

            repository.Update(project, CurrentApplicationUser);
            repository.SaveChanges();
            
            return GetResponseSuccess(message: Resources.ProjectUpdatedSuccessfully);
        }
    }
}