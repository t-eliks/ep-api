using DataAccess.Entities.Junctions;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Project : PersistentEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Epic> Epics { get; set; }

        public virtual List<ProjectToUserJunction> Collaborators { get; set; } = new List<ProjectToUserJunction>();
    }
}
