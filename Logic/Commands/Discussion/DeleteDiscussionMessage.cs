using System.Linq;
using DataAccess;
using DataAccess.Models;
using Logic.Base;
using Logic.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Discussion
{
    public class DeleteDiscussionMessageCommand : BaseCommand<GenericRequestViewModel<int>, EmptyViewModel>
    {
        private Repository repository;

        private readonly IHubContext<DiscussionHub> discussionHub;
        
        public DeleteDiscussionMessageCommand(Repository repository, IHubContext<DiscussionHub> discussionHub)
        {
            this.repository = repository;
            this.discussionHub = discussionHub;
        }
        
        protected override BaseResponse<EmptyViewModel> ExecuteCore(GenericRequestViewModel<int> request)
        {
            var discussionMessage = repository.DiscussionMessages
                .Read(x => x.Id == request.Data)
                .Include(x => x.CreatedBy)
                .Include(x => x.Project)
                .SingleOrDefault();

            if (discussionMessage?.CreatedBy != CurrentApplicationUser)
            {
                return GetGenericResponseFailed();
            }
            discussionHub.Clients.Group(discussionMessage.Project.Id.ToString())
                .SendAsync("MessageRemoved", discussionMessage.Id.ToString());

            repository.Delete(discussionMessage, CurrentApplicationUser);
            repository.SaveChanges();

            return GetGenericResponseSuccess(null);
        }
    }
}