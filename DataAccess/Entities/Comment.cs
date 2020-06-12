using DataAccess.Entities.User;
using System;

namespace DataAccess.Entities
{
    public class Comment : PersistentEntity
    {
        public string Content { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
