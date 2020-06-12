using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Discussion
{
    public class GetNotificationsCommand : BaseCommand<GenericRequestViewModel<int>, IList<NotificationDto>>
    {
        private readonly Repository repository;

        public GetNotificationsCommand(Repository repository)
        {
            this.repository = repository;
        }

        protected override BaseResponse<IList<NotificationDto>> ExecuteCore(GenericRequestViewModel<int> request)
        {
            var notifications = repository.Notifications
                .Read(x => x.User == CurrentApplicationUser && x.Project.Id == request.Data)
                .Include(x => x.Project)
                .ToList();

            var notificationDtos = notifications.Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    IsRead = x.IsRead,
                    CreatedOn = x.CreatedOn,
                    Type = x.Type
                })
                .OrderBy(x => x.IsRead)
                .ThenByDescending(x => x.CreatedOn)
                .ToList();

            notifications.ForEach(x => x.IsRead = true);

            repository.SaveChanges();

            return GetGenericResponseSuccess(notificationDtos);
        }
    }
}