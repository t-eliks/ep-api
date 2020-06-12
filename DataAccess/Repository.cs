using DataAccess.Entities;
using DataAccess.Entities.Junctions;
using DataAccess.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class Repository : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public Repository(DbContextOptions<Repository> options) : base(options)
        {
            
        }

        public new virtual DbSet<ApplicationUser> Users { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Epic> Epics { get; set; }

        public virtual DbSet<Assignment> Assignments { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<ProjectToUserJunction> ProjectToUser { get; set; }

        public virtual DbSet<Invitation> Invitations { get; set; }

        public virtual DbSet<DiscussionMessage> DiscussionMessages { get; set; }
        
        public virtual DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTableNames()
                .ConfigureApplicationUser()
                .ConfigureEntities();
        }
    }
}
