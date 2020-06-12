using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Entities;
using DataAccess.Entities.Junctions;
using DataAccess.Entities.User;
using DataAccess.Enums;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public static class RepositoryExtensions
    {
        private const string ProjectSchema = "Project";
        private const string InteractivitySchema = "Interactivity";
        private const string UserSchema = "User";
        private const string JunctionSchema = "Junction";

        public static int Create(this Repository context, IEntity entity, ApplicationUser createdBy)
        {
            entity.CreatedBy = createdBy;
            entity.CreatedOn = DateTime.UtcNow;

            context.Add(entity);

            return entity.Id;
        }

        public static IQueryable<TEntity> Read<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> expr) where TEntity : class, IEntity
        {
            return set.Where(expr);
        }

        public static IQueryable<TEntity> ReadNotDeleted<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> expr = null) where TEntity: class, IPersistentEntity
        {
            var query = set.Where(x => !x.DeletedOn.HasValue);

            if (expr != null)
                query = query.Where(expr);

            return query;
        }

        public static int Update(this Repository context, IEntity entity, ApplicationUser modifiedBy)
        {
            entity.ModifiedBy = modifiedBy;
            entity.ModifiedOn = DateTime.UtcNow;

            context.Update(entity);

            return entity.Id;
        }

        /// <summary>
        /// Removes entity from database. Soft-deletes if entity is <see cref="IPersistentEntity"/>
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="entity"><see cref="IEntity"/> to be deleted</param>
        /// <param name="deletedBy"><see cref="ApplicationUser"/> who is performing the deletion operation</param>
        /// <returns><see cref="IPersistentEntity.Id"/> if entity was soft-deleted, 0 otherwise</returns>
        public static int Delete(this Repository context, IEntity entity, ApplicationUser deletedBy)
        {
            if (!(entity is IPersistentEntity persistentEntity))
            {
                context.Remove(entity);

                return 0;
            }

            persistentEntity.DeletedBy = deletedBy;
            persistentEntity.DeletedOn = DateTime.UtcNow;

            return persistentEntity.Id;
        }

        public static IList<TEntity> GetUserEntities<TEntity>(this DbSet<TEntity> set, ApplicationUser user, Expression<Func<TEntity, bool>> expr = null) 
            where TEntity: class, IEntity
        {
            var compiledExpr = expr?.Compile();

            var allEntities = set.ToList();
            
            var filteredEntitiesQuery = allEntities.Where(x => x.CreatedBy == user || 
                                                          x.ModifiedBy == user || 
                                                          (compiledExpr != null && compiledExpr.Invoke(x))
                                                          || (x is IPersistentEntity && ((IPersistentEntity)x).DeletedBy == user));

            return filteredEntitiesQuery.ToList();
        }

        public static ModelBuilder ConfigureTableNames(this ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("ApplicationUser", UserSchema);
            builder.Entity<Assignment>().ToTable("Assignment", ProjectSchema);
            builder.Entity<Comment>().ToTable("Comment", InteractivitySchema);
            builder.Entity<Project>().ToTable("Project",  ProjectSchema);
            builder.Entity<Epic>().ToTable("Epic", ProjectSchema);
            builder.Entity<ProjectToUserJunction>().ToTable("ProjectToUser", JunctionSchema);
            builder.Entity<Invitation>().ToTable("Invitation", ProjectSchema);
            builder.Entity<DiscussionMessage>().ToTable("DiscussionMessage", InteractivitySchema);
            builder.Entity<Notification>().ToTable("Notification", InteractivitySchema);

            return builder;
        }

        public static ModelBuilder ConfigureApplicationUser(this ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .Ignore(x => x.NormalizedEmail)
                .Ignore(x => x.NormalizedUserName)
                .Ignore(x => x.PhoneNumber)
                .Ignore(x => x.PhoneNumberConfirmed)
                .Ignore(x => x.TwoFactorEnabled)
                .Ignore(x => x.LockoutEnabled)
                .Ignore(x => x.LockoutEnd)
                .Ignore(x => x.AccessFailedCount);

            builder.Ignore<IdentityRoleClaim<int>>();
            builder.Ignore<IdentityUserRole<int>>();
            builder.Ignore<IdentityUserLogin<int>>();
            builder.Ignore<IdentityUserToken<int>>();
            builder.Ignore<IdentityUserClaim<int>>();
            builder.Ignore<IdentityRole>();

            return builder;
        }

        public static ModelBuilder ConfigureEntities(this ModelBuilder builder)
        {
            builder.Entity<Project>()
                .HasMany(x => x.Collaborators)
                .WithOne(x => x.Project)
                .OnDelete(DeleteBehavior.Cascade);
            
            return builder;
        }

        public static void SeedDatabase(this Repository repository)
        {
            if (!repository.Database.EnsureCreated())
            {
                return;
            }

            var shirley = new ApplicationUser
            {
                Email = "demoShirley@easyproject.com",
                FirstName = "Shirley",
                LastName = "R. Williams",
                UserName = "demoShirley@easyproject.com",
                PasswordHash = "AQAAAAEAACcQAAAAEAEcVubuaDLzBYYNu7aCOvJZjP5r696jOWZMUS7NY0FLj22g5opgFhLukAfti+CtjA=="
            };
            
            var stephen = new ApplicationUser
            {
                Email = "demoStephen@easyproject.com",
                FirstName = "Stephen",
                LastName = "Bryant",
                UserName = "demoStephen@easyproject.com",
                PasswordHash = "AQAAAAEAACcQAAAAEKZxLV3C2lNtOU9J4f+bT3AUAnIGF3P2Ghlyp2b85KCKFW8pYH5L07SW6QBmP//Odw=="
            };
            
            var reginald = new ApplicationUser
            {
                Email = "demoReginald@easyproject.com",
                FirstName = "Reginald",
                LastName = "Ottinger",
                UserName = "demoReginald@easyproject.com",
                PasswordHash = "AQAAAAEAACcQAAAAEDWoMxKTx2rllIJ5qXh8HwiZEpKAWvHcRTWblWDOia3PeOKADScHeLUoL05/M9n8UQ=="
            };

            var aubrey = new ApplicationUser
            {
                Email = "demoAubrey@easyproject.com",
                FirstName = "Aubrey",
                LastName = "Fitzgerald",
                UserName = "demoAubrey@easyproject.com",
                PasswordHash = "AQAAAAEAACcQAAAAEAdKDbzyGhsrb9P7XbBYJzUqCBI4KAomwynQAs73kpgc45Z1Xy7oedZCNvOnBoIg1A=="
            };
            
            var project = new Project
            {
                Name = "SmartShirt",
                Description = "Utilising modern sensors and fabrics, this product will enable users to track their body vitals with unparalleled accuracy" +
                              " and precision, while not inhibiting their movement or constraining their bodies while exercising.",
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(15))
            };
            
            var marketResearchEpic = new Epic
            {
                Name = "Market Research",
                Description = "Conduct in depth market research, align product features and customer expectations, assert the viability of our product.",
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                Project = project
            };

            var marketResearchAssignment1 = new Assignment
            {
                Name = "[MR] Create 3 hypotheses on client needs in body sensor technology.",
                Description = "These hypotheses will be used in further market research and viability assertions.",
                Deadline = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                Assignee = shirley,
                Status = AssignmentStatus.Complete,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(14)),
                Epic = marketResearchEpic
            };

            var marketResearchAssignment2 = new Assignment
            {
                Name = "[MR] Assert hypotheses by conducting client interviews",
                Deadline = DateTime.Now.Subtract(TimeSpan.FromDays(3)),
                Assignee = shirley,
                Status = AssignmentStatus.InProgress,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(12)),
                ModifiedBy = shirley,
                ModifiedOn = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                Epic = marketResearchEpic

            };

            var marketResearchAssignment3 = new Assignment
            {
                Name = "[MR] Investigate competitor products",
                Description =
                    "We should look into other similar products and come up with unique features with which we could attract a customer base.",
                Status = AssignmentStatus.Todo,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(14)),
                Epic = marketResearchEpic
            };

            var productDevelopmentEpic = new Epic
            {
                Name = "Product Development",
                Description = "Everything related to product production needs.",
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                Project = project
            };

            var productDevelopmentAssignment1 = new Assignment
            {
                Name = "[PD] Find contractors for production line services",
                Assignee = reginald,
                Status = AssignmentStatus.InProgress,
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                ModifiedBy = stephen,
                ModifiedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6)),
                Epic = productDevelopmentEpic
            };

            var productDevelopmentAssignment2 = new Assignment
            {
                Name = "[PD] Design schematics for first prototype",
                Assignee = stephen,
                Status = AssignmentStatus.Todo,
                CreatedBy = reginald,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                ModifiedBy = stephen,
                ModifiedOn = DateTime.Now.Subtract(TimeSpan.FromDays(8)),
                Epic = productDevelopmentEpic
            };

            var intellectualPropertyEpic = new Epic
            {
                Name = "Intellectual Property",
                Description = "Everything related to intellectual property rights.",
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(14)),
                Project = project
            };

            var intellectualPropertyAssignment1 = new Assignment
            {
                Name = "[IP] Investigate requirements for patent registration",
                Description =
                    "We should make sure we meet the criteria for a patent ASAP to make sure we can start promoting our product without fear of " +
                    "intellectual theft",
                Assignee = stephen,
                Status = AssignmentStatus.InProgress,
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                Epic = intellectualPropertyEpic
            };

            var shirleyJunction = new ProjectToUserJunction
            {
                Collaborator = shirley,
                Project = project,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now
            };
            
            var stephenJunction = new ProjectToUserJunction
            {
                Collaborator = stephen,
                Project = project,
                CreatedBy = stephen,
                CreatedOn = DateTime.Now
            };
            
            var reginaldJunction = new ProjectToUserJunction
            {
                Collaborator = reginald,
                Project = project,
                CreatedBy = reginald,
                CreatedOn = DateTime.Now
            };

            var comment1 = new Comment
            {
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
                Content =
                    "{\"blocks\":[{\"key\":\"9o9oo\",\"text\":\"Perhaps it would be a good idea to collect information on the interviewees\' workout habits? That way we would be able to more accurately assess the opinions of people who have a more active lifestyle.\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}",
                Assignment = marketResearchAssignment2
            };

            var notification1 = new Notification
            {
                IsRead = false,
                Type = NotificationType.NewComment,
                Content =
                    "A new comment on assignment [ [MR] Assert hypotheses by conducting client interviews ] has been left by: Stephen Bryant",
                User = shirley,
                Project = project,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(7))
            };

            var comment2 = new Comment
            {
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6)),
                Content =
                    "{\"blocks\":[{\"key\":\"42eev\",\"text\":\"Good idea, I will make sure to inquire about it!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}",
                Assignment = marketResearchAssignment2
            };

            var discussionMessage1 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6).Subtract(TimeSpan.FromHours(15).Subtract(TimeSpan.FromMinutes(30)))),
                Content =
                    "{\"blocks\":[{\"key\":\"2jfo2\",\"text\":\"Hey guys, welcome to our project board! Don\'t hesitate to use this chat to discuss anything, it will be useful to have everything in one place whenever!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage2 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6).Subtract(TimeSpan.FromHours(15).Subtract(TimeSpan.FromMinutes(29)))),
                Content = "{\"blocks\":[{\"key\":\"c63tm\",\"text\":\"Hey Stephen! Actually I had a question already: have we decided on our initial steps yet? I propose focusing on market research before we commit to anything. For this kind of product, a prototype will be quite expensive, we should make sure our first one is as close to the real one as we can!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage3 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6).Subtract(TimeSpan.FromHours(15).Subtract(TimeSpan.FromMinutes(27)))),
                Content = "{\"blocks\":[{\"key\":\"a0shc\",\"text\":\"Good point. I will create some assignments for that right away. I suspect you\'ll want to volunteer for this one?\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage4 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6).Subtract(TimeSpan.FromHours(15).Subtract(TimeSpan.FromMinutes(26)))),
                Content = "{\"blocks\":[{\"key\":\"des4c\",\"text\":\"Yes, you can assign me right away!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage5 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = reginald,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6).Subtract(TimeSpan.FromHours(16).Subtract(TimeSpan.FromMinutes(30)))),
                Content = "{\"blocks\":[{\"key\":\"b88bn\",\"text\":\"Hey guys, good to hear from you! I see everyone\'s eager to start working haha. Stephen, I think we should also prioritise intellectual property asap, considering we will want to start showing this product off at demos, we need to make sure we have a patent process underway. Don\'t want anyone snatching this from under our noses haha!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage6 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6).Subtract(TimeSpan.FromHours(16).Subtract(TimeSpan.FromMinutes(25)))),
                Content = "{\"blocks\":[{\"key\":\"17gb\",\"text\":\"Hey Reginald, good idea! I\'ll start looking into it as soon as I have time!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage7 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(3).Subtract(TimeSpan.FromHours(16).Subtract(TimeSpan.FromMinutes(30)))),
                Content = "{\"blocks\":[{\"key\":\"4o5dv\",\"text\":\"Hey, anyone up for some lunch? I know a good place 15min away!!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage8 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = stephen,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(3).Subtract(TimeSpan.FromHours(16).Subtract(TimeSpan.FromMinutes(29)))),
                Content = "{\"blocks\":[{\"key\":\"fmi0o\",\"text\":\"I\'d join! Didn\'t realise how hungry I was \'till you mentioned it :D\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage9 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = reginald,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(3).Subtract(TimeSpan.FromHours(16).Subtract(TimeSpan.FromMinutes(28)))),
                Content = "{\"blocks\":[{\"key\":\"6v9h6\",\"text\":\"Same here! See ya by the exit in 10?\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };
            
            var discussionMessage10 = new DiscussionMessage
            {
                Project = project,
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(3).Subtract(TimeSpan.FromHours(16).Subtract(TimeSpan.FromMinutes(27)))),
                Content = "{\"blocks\":[{\"key\":\"6m7rc\",\"text\":\"Deal!\",\"type\":\"unstyled\",\"depth\":0,\"inlineStyleRanges\":[],\"entityRanges\":[],\"data\":{}}],\"entityMap\":{}}"
            };

            var invitation = new Invitation
            {
                Email = aubrey.Email,
                Project = project,
                Token = Guid.NewGuid().ToString(),
                CreatedBy = shirley,
                CreatedOn = DateTime.Now.Subtract(TimeSpan.FromDays(6))
            };

            repository.Add(shirley);
            repository.Add(reginald);
            repository.Add(stephen);
            repository.Add(aubrey);
            
            project.Collaborators.Add(shirleyJunction);
            project.Collaborators.Add(stephenJunction);
            project.Collaborators.Add(reginaldJunction);

            repository.Add(project);
            
            repository.Add(marketResearchEpic);
            repository.Add(marketResearchAssignment1);
            repository.Add(marketResearchAssignment2);
            repository.Add(marketResearchAssignment3);

            repository.Add(productDevelopmentEpic);
            repository.Add(productDevelopmentAssignment1);
            repository.Add(productDevelopmentAssignment2);
            
            repository.Add(intellectualPropertyEpic);
            repository.Add(intellectualPropertyAssignment1);

            repository.Add(comment1);
            repository.Add(comment2);

            repository.Add(notification1);

            repository.Add(discussionMessage1);
            repository.Add(discussionMessage2);
            repository.Add(discussionMessage3);
            repository.Add(discussionMessage4);
            repository.Add(discussionMessage5);
            repository.Add(discussionMessage6);
            repository.Add(discussionMessage7);
            repository.Add(discussionMessage8);
            repository.Add(discussionMessage9);
            repository.Add(discussionMessage10);

            repository.Add(invitation);
            
            repository.SaveChanges();
        }
    }
}
