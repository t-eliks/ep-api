namespace DataAccess.Entities
{
    public class DiscussionMessage : BaseEntity
    {
        public virtual Project Project { get; set; }

        public string Content { get; set; }
    }
}
