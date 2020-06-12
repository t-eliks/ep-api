using DataAccess.Models;
using Logic.Base;
using Logic.CommandFactory;
using Logic.Commands.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ExtendedController
    {
        public ProjectController(ICommandFactory commandFactory) : base(commandFactory) { }

        [Authorize]
        [HttpPost]
        public ActionResult<BaseResponse<EmptyViewModel>> CreateProject([FromBody] NewProjectViewModel model)
        {
            return GetResult(GetCommand<CreateProjectCommand>().Execute(model));
        }
        
        [Authorize]
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> UpdateProject([FromRoute] int id, [FromBody] NewProjectViewModel model)
        {
            return GetResult(GetCommand<UpdateProjectCommand>().Execute(new EditProjectRequestViewModel
            {
               ProjectId = id,
               ViewModel = model
            }));
        }
        
        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> DeleteProject([FromRoute] int id)
        {
            return GetResult(GetCommand<DeleteProjectCommand>().Execute(new GenericRequestViewModel<int>()
            {
                Data = id
            }));
        }

        [Authorize]
        [HttpGet]
        [Route("details/{projectId}")]
        public ActionResult<BaseResponse<ProjectDetailsViewModel>> GetProjectDetails([FromRoute] int projectId)
        {
            return GetResult(GetCommand<GetProjectDetailsCommand>().Execute(new ProjectInfoRequestViewModel
            {
                ProjectId = projectId
            }));
        }
    }
}