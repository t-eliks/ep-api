using DataAccess.Entities.User;

namespace Services.Interfaces.Notification
{
    public interface INotificationService
    {
        void CreateAssignedToTaskNotification(ApplicationUser assignedBy, ApplicationUser assignee,
            DataAccess.Entities.Assignment assignment);

        void CreateAssignmentUpdatedNotification(ApplicationUser updatedBy, ApplicationUser recipient,
            DataAccess.Entities.Assignment assignment);

        void CreateNewCommentNotification(ApplicationUser leftBy, ApplicationUser recipient,
            DataAccess.Entities.Assignment assignment);
    }
}