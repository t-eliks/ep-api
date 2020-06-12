using System;
using System.Collections.Generic;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Entities.User;

namespace Services.Interfaces.Assignment
{
    public interface IAssignmentService
    {
        void CreateAssignment(ApplicationUser createdBy, ApplicationUser assignee, Epic epic, DateTime? deadline, string name, string description);

        List<DataAccess.Entities.Assignment> GetAssignments(DataAccess.Entities.Project project);

        AssignmentDto GetAssignmentDto(DataAccess.Entities.Assignment assignment);

        DataAccess.Entities.Assignment GetAssignment(int assignmentId);
    }
}
