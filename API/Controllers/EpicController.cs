using DataAccess.Models;
using Logic.Base;
using Logic.CommandFactory;
using Logic.Commands.Epic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpicController : ExtendedController
    {
        public EpicController(ICommandFactory commandFactory) : base(commandFactory) { }

        [Authorize]
        [HttpPost]
        public ActionResult<BaseResponse<EmptyViewModel>> CreateEpic([FromBody] NewEditEpicViewModel model)
        {
            return GetResult(GetCommand<CreateEpicCommand>().Execute(model));
        }

        [Authorize]
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> EditEpic([FromRoute] int id, [FromBody] NewEditEpicViewModel model)
        {
            return GetResult(GetCommand<UpdateEpicCommand>().Execute(new EditEpicRequestViewModel
            {
               ViewModel = model,
               EpicId = id
            }));
        }
        
        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> DeleteEpic([FromRoute] int id)
        {
            return GetResult(GetCommand<DeleteEpicCommand>().Execute(new GenericRequestViewModel<int>
            {
                Data = id
            }));
        }

        [Authorize]
        [HttpGet]
        [Route("backlog/{projectId}")]
        public ActionResult<BaseResponse<BacklogViewModel>> GetProjectBacklog([FromRoute]int projectId)
        {
            return GetResult(GetCommand<GetProjectBacklogCommand>().Execute(new ProjectInfoRequestViewModel
            {
                ProjectId = projectId
            }));
        }
    }
}