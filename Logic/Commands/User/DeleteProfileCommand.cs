using System.Linq;
using DataAccess;
using DataAccess.Models;
using Localization;
using Logic.Base;

namespace Logic.Commands.User
{
    public class DeleteProfileCommand : BaseCommand<EmptyViewModel>
    {
        private readonly Repository repository;

        public DeleteProfileCommand(Repository repository)
        {
            this.repository = repository;
        }

        protected override BaseResponse<EmptyViewModel> ExecuteCore()
        {
            repository.ProjectToUser.GetUserEntities(CurrentApplicationUser, x => x.Collaborator == CurrentApplicationUser)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.Comments.GetUserEntities(CurrentApplicationUser)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.DiscussionMessages.GetUserEntities(CurrentApplicationUser)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.Assignments.GetUserEntities(CurrentApplicationUser, x => x.Assignee == CurrentApplicationUser)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.Projects.GetUserEntities(CurrentApplicationUser)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.Epics.GetUserEntities(CurrentApplicationUser)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.Notifications.GetUserEntities(CurrentApplicationUser, x => x.User == CurrentApplicationUser)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.Invitations.GetUserEntities(CurrentApplicationUser, x => x.Email == CurrentApplicationUser.Email)
                .ToList()
                .ForEach(x => repository.Remove(x));
            
            repository.Remove(CurrentApplicationUser);

            repository.SaveChanges();

            return GetResponseSuccess(message: Resources.ProfileDeletedSuccessfully);
        }
    }
}