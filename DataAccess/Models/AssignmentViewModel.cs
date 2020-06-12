using DataAccess.Dtos;
using DataAccess.Enums;
using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public class AssignmentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AssignmentStatus Status { get; set; }

        public DateTime? Deadline { get; set; }

        public string AssigneeId { get; set; }

        public DateTime CreatedOn { get; set; }

        public IList<CommentDto> Comments { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string LastModifiedFirstName { get; set; }

        public string LastModifiedLastName { get; set; }
        
        public string Description { get; set; }
    }
}
