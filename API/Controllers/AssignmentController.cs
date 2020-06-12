using DataAccess.Models;
using Logic.Base;
using Logic.CommandFactory;
using Logic.Commands.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ExtendedController
    {
        public AssignmentController(ICommandFactory commandFactory) : base(commandFactory) { }

        [Authorize]
        [HttpPost]
        public ActionResult<BaseResponse<EmptyViewModel>> CreateAssignment([FromBody] NewAssignmentViewModel model)
        {
            return GetResult(GetCommand<CreateAssignmentCommand>().Execute(model));
        }


        [Authorize]
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> UpdateAssignment([FromRoute] int id, [FromBody] NewAssignmentViewModel model)
        {
            return GetResult(GetCommand<UpdateAssignmentCommand>().Execute(new EditAssignmentRequestViewModel()
            {
                ViewModel = model,
                AssignmentId = id
            }));
        }

        [Authorize]
        [HttpPost]
        [Route("update-status")]
        public ActionResult<BaseResponse<EmptyViewModel>> UpdateAssignmentStatus([FromBody] UpdateAssignmentStatusRequestViewModel viewModel)
        {
            return GetResult(GetCommand<UpdateAssignmentStatusCommand>().Execute(viewModel));
        }
    
        [Authorize]
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<AssignmentViewModel>> GetAssignment([FromRoute] int id)
        {
            return GetResult(GetCommand<GetAssignmentCommand>().Execute(new AssignmentInfoRequestViewModel
            {
                AssignmentId = id
            }));
        }

        [Authorize]
        [HttpGet]
        [Route("board/{projectId}")]
        public ActionResult<BaseResponse<BoardViewModel>> GetBoard([FromRoute] int projectId)
        {
            return GetResult(GetCommand<GetProjectBoardCommand>().Execute(new ProjectInfoRequestViewModel
            {
                ProjectId = projectId
            }));
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> DeleteAssignment([FromRoute] int id)
        {
            return GetResult(GetCommand<DeleteAssignmentCommand>().Execute(new GenericRequestViewModel<int>
            {
                Data = id
            }));
        }
    }
}