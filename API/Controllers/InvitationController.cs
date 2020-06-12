using System.Collections.Generic;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Logic.CommandFactory;
using Logic.Commands.Invitation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ExtendedController
    {
        public InvitationController(ICommandFactory commandFactory) : base(commandFactory) { }

        [HttpPost]
        public ActionResult<BaseResponse<string>> GenerateInvitation([FromBody] InvitationRequestViewModel model)
        {
            return GetResult(GetCommand<GenerateInvitationCommand>().Execute(model));
        }

        [HttpGet]
        [Route("pending-invitations/{projectId:int}")]
        public ActionResult<BaseResponse<IList<PendingInvitationDto>>> GetPendingInvitations([FromRoute] int projectId)
        {
            return GetResult(GetCommand<GetPendingInvitationsCommand>().Execute(new GenericProjectRequestViewModel
            {
                ProjectId = projectId
            }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{token}")]
        public ActionResult<BaseResponse<PendingInvitationDto>> GetPendingInvitationInfo([FromRoute] string token)
        {
            return GetResult(GetCommand<GetPendingInvitationInfoCommand>().Execute(new PendingInvitationRequestViewModel
            {
                Token = token
            }));
        }

        [HttpPost]
        [Route("accept/{token}")]
        public ActionResult<BaseResponse<EmptyViewModel>> AcceptInvitation([FromRoute] string token)
        {
            return GetResult(GetCommand<AcceptInvitationCommand>().Execute(new PendingInvitationRequestViewModel
            {
                Token = token
            }));
        }

        [HttpPost]
        [Route("cancel/{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> CancelInvitation([FromRoute] int id)
        {
            return GetResult(GetCommand<CancelInvitationCommand>().Execute(new GenericRequestViewModel<int>
            {
                Data = id
            }));
        }
        
        [Authorize]
        [HttpGet]
        [Route("collaborators/{projectId}")]
        public ActionResult<BaseResponse<CollaboratorOverviewViewModel>> GetCollaboratorOverview([FromRoute] string projectId)
        {
            return GetResult(GetCommand<GetCollaboratorOverviewCommand>().Execute(new CollaboratorOverviewRequestViewModel
            {
                ProjectId = projectId
            }));
        }
    }
}
