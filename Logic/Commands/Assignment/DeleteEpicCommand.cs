using System.Linq;
using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Logic.Commands.Assignment
{
    public class DeleteAssignmentCommand : BaseCommand<GenericRequestViewModel<int>, EmptyViewModel>
    {
        private readonly Repository repository;
        
        private readonly IProjectService projectService;
        
        public DeleteAssignmentCommand(IProjectService projectService, Repository repository)
        {
            this.projectService = projectService;
            this.repository = repository;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(GenericRequestViewModel<int> request)
        {
            var assignment = repository
                .Assignments
                .ReadNotDeleted(x => x.Id == request.Data)
                .Include(x => x.Epic)
                .ThenInclude(x => x.Project)
                .SingleOrDefault();

            if (assignment is null)
            {
                return GetGenericResponseFailed();
            }
            
            var validationResult = ValidateProjectAccess(projectService, assignment.Epic.Project.Id);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }

            repository.Delete(assignment, CurrentApplicationUser);
            
            repository.SaveChanges();
            
            return GetResponseSuccess(message: Resources.AssignmentDeletedSuccessfully);
        }
    }
}