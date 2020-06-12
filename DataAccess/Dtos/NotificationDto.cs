using System;
using DataAccess.Enums;

namespace DataAccess.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        
        public string Content { get; set; }
        
        public bool IsRead { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public NotificationType Type { get; set; }
        
        public int AssignmentId { get; set; }
    }
}