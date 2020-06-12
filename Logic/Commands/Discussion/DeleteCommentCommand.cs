using System.Linq;
using DataAccess;
using DataAccess.Models;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Logic.Commands.Discussion
{
    public class DeleteCommentCommand : BaseCommand<GenericRequestViewModel<int>, EmptyViewModel>
    {
        private readonly IProjectService projectService;

        private readonly Repository repository;

        public DeleteCommentCommand(IProjectService projectService, Repository repository)
        {
            this.projectService = projectService;
            this.repository = repository;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(GenericRequestViewModel<int> request)
        {
            var comment = repository.Comments
                .ReadNotDeleted(x => x.Id == request.Data)
                .Include(x => x.CreatedBy)
                .SingleOrDefault();

            if (comment == null || comment.CreatedBy != CurrentApplicationUser)
            {
                return GetGenericResponseFailed();
            }

            repository.Delete(comment, CurrentApplicationUser);
            repository.SaveChanges();

            return GetGenericResponseSuccess(null);
        }
    }
}