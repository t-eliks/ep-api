using System.Linq;
using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Services.Interfaces.Project;

namespace Logic.Commands.Project
{
    public class DeleteProjectCommand : BaseCommand<GenericRequestViewModel<int>, EmptyViewModel>
    {
        private readonly Repository repository;
        
        private readonly IProjectService projectService;
        
        public DeleteProjectCommand(IProjectService projectService, Repository repository)
        {
            this.projectService = projectService;
            this.repository = repository;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(GenericRequestViewModel<int> request)
        {
            var project = repository
                .Projects
                .ReadNotDeleted(x => x.Id == request.Data)
                .SingleOrDefault();

            if (project is null)
            {
                return GetGenericResponseFailed();
            }
            
            var validationResult = ValidateProjectAccess(projectService, project.Id);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }

            if (project.CreatedBy != CurrentApplicationUser)
            {
                return GetGenericResponseFailed();
            }

            repository.Delete(project, CurrentApplicationUser);
            repository.SaveChanges();
            
            return GetResponseSuccess(message: Resources.ProjectDeletedSuccessfully);
        }
    }
}