using DataAccess;
using DataAccess.Entities.User;
using DataAccess.Enums;
using Services.Interfaces.Notification;

namespace Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly Repository repository;

        public NotificationService(Repository repository)
        {
            this.repository = repository;
        }

        public void CreateAssignedToTaskNotification(ApplicationUser assignedBy, ApplicationUser assignee,
            DataAccess.Entities.Assignment assignment)
        {
            var content = $"You have been assigned to task [ {assignment.Name} ] by: {assignedBy.FullName}";

            var notification = new DataAccess.Entities.Notification
            {
                Content = content,
                CreatedBy = assignedBy,
                User = assignee,
                Project = assignment.Epic.Project,
                Type = NotificationType.AssigneeChange
            };

            repository.Create(notification, assignee);
        }
        
        public void CreateAssignmentUpdatedNotification(ApplicationUser updatedBy, ApplicationUser recipient,
            DataAccess.Entities.Assignment assignment)
        {
            var content = $"Assignment [ {assignment.Name} ] has been updated by: {updatedBy.FullName}";

            var notification = new DataAccess.Entities.Notification
            {
                Content = content,
                CreatedBy = updatedBy,
                User = recipient,
                Project = assignment.Epic.Project,
                Type = NotificationType.AssignmentUpdate
            };

            repository.Create(notification, recipient);
        }
        
        public void CreateNewCommentNotification(ApplicationUser leftBy, ApplicationUser recipient,
            DataAccess.Entities.Assignment assignment)
        {
            var content = $"A new comment on assignment [ {assignment.Name} ] has been left by: {leftBy.FullName}";

            var notification = new DataAccess.Entities.Notification
            {
                Content = content,
                CreatedBy = leftBy,
                User = recipient,
                Project = assignment.Epic.Project,
                Type = NotificationType.NewComment
            };

            repository.Create(notification, recipient);
        }
    }
}