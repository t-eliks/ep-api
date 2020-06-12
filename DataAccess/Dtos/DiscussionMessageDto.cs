using System;

namespace DataAccess.Dtos
{
    public class DiscussionMessageDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }
        
        public bool IsAuthor { get; set; }
    }
}
