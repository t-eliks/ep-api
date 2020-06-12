using DataAccess.Dtos;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public class BoardViewModel
    {
        public IList<BoardAssignmentDto> Assignments { get; set; }
    }
}
