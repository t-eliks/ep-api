using System;

namespace DataAccess.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        public bool IsAssignee { get; set; }
        
        public bool IsAuthor { get; set; }

        public int AssignmentId { get; set; }
    }
}
