using DataAccess.Dtos;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public class DashboardViewModel
    {
        public IList<DashboardProjectDto> ProjectDtos { get; set; }

        // TODO: LATEST ACTIVITY ETC...
    }
}
