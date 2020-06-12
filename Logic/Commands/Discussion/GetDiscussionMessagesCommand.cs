using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Project;

namespace Logic.Commands.Discussion
{
    public class GetDiscussionMessagesCommand : BaseCommand<GetDiscussionMessagesRequestViewModel,IList<DiscussionMessageDto>>
    {
        private readonly IProjectService projectService;

        private readonly Repository repository;

        public GetDiscussionMessagesCommand(IProjectService projectService, Repository repository)
        {
            this.projectService = projectService;
            this.repository = repository;
        }

        protected override BaseResponse<IList<DiscussionMessageDto>> ExecuteCore(GetDiscussionMessagesRequestViewModel request)
        {
            var validationResult = ValidateProjectAccess(projectService, request.ProjectId);

            if (!validationResult.Response.Success)
            {
                return validationResult.Response;
            }

            var messagesQuery = repository
                .DiscussionMessages
                .Read(x => x.Project == validationResult.Project)
                .Include(x => x.Project)
                .Include(x => x.CreatedBy)
                .Select(x => new DiscussionMessageDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    AuthorName = string.Join(' ', x.CreatedBy.FirstName, x.CreatedBy.LastName),
                    CreatedOn = x.CreatedOn,
                    IsAuthor = x.CreatedBy == CurrentApplicationUser
                });

            if (request.MessageId.HasValue)
            {
                messagesQuery = messagesQuery.Where(x => x.Id == request.MessageId);
            }

            var messages = messagesQuery
                .OrderBy(x => x.CreatedOn)
                .ToList();

            return GetGenericResponseSuccess(messages);
        }
    }
}
