using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Epic : PersistentEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Project Project { get; set; }

        public virtual List<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
