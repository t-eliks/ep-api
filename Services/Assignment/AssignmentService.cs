using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Entities.User;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Assignment;
using Services.Interfaces.Notification;

namespace Services.Assignment
{
    public class AssignmentService : IAssignmentService
    {
        private Repository repository;

        private readonly INotificationService notificationService;
        
        public AssignmentService(Repository repository, INotificationService notificationService)
        {
            this.repository = repository;
            this.notificationService = notificationService;
        }

        public void CreateAssignment(ApplicationUser createdBy, ApplicationUser assignee, Epic epic, DateTime? deadline, string name, string description)
        {
            var assignment = new DataAccess.Entities.Assignment
            {
                Name = name,
                Description = description,
                Assignee = assignee,
                Deadline = deadline,
                Epic = epic
            };

            repository.Create(assignment, createdBy);

            if (assignee != null && createdBy != assignee)
            {
                notificationService.CreateAssignedToTaskNotification(createdBy, assignee, assignment);
            }
        }

        public List<DataAccess.Entities.Assignment> GetAssignments(DataAccess.Entities.Project project)
        {
            return repository.Assignments
                .ReadNotDeleted()
                .Include(x => x.Comments)
                .Include(x => x.Epic)
                .ThenInclude(x => x.Project)
                .Where(x => x.Epic.Project == project)
                .ToList();
        }

        public AssignmentDto GetAssignmentDto(DataAccess.Entities.Assignment assignment)
        {
            return new AssignmentDto
            {
                Id = assignment.Id,
                AssigneeFirstName = assignment.Assignee?.FirstName,
                AssigneeLastName = assignment.Assignee?.LastName,
                Name = assignment.Name,
                Deadline = assignment?.Deadline,
                Status = assignment.Status
            };
        }

        public DataAccess.Entities.Assignment GetAssignment(int assignmentId)
        {
            return repository.Assignments.ReadNotDeleted(x => x.Id == assignmentId)
                .Include(x => x.CreatedBy)
                .Include(x => x.Epic)
                .ThenInclude(x => x.Project)
                .ThenInclude(x => x.Collaborators)
                .SingleOrDefault();
        }
    }
}
