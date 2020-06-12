using DataAccess.Entities.User;
using DataAccess.Enums;

namespace DataAccess.Entities
{
    public class Notification : BaseEntity
    {
        public virtual Project Project { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        
        public string Content { get; set; }
        
        public bool IsRead { get; set; }
        
        public NotificationType Type { get; set; }
    }
}