using DataAccess.Dtos;
using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public class BacklogViewModel
    {
        public IList<EpicBacklogDto> Epics { get; set; }

        public DateTime? EarliestDeadline { get; set; }

        public int? EarliestDeadlineAssignmentId { get; set; }
    }
}
