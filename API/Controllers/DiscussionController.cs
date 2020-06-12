using System.Collections.Generic;
using DataAccess.Dtos;
using DataAccess.Models;
using Logic.Base;
using Logic.CommandFactory;
using Logic.Commands.Discussion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ExtendedController
    {
        public DiscussionController(ICommandFactory commandFactory) : base(commandFactory) { }

        [Authorize]
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<IList<DiscussionMessageDto>>> GetDiscussionMessages([FromRoute] int id, [FromQuery] int? messageId)
        {
            return GetResult(GetCommand<GetDiscussionMessagesCommand>().Execute(new GetDiscussionMessagesRequestViewModel
            {
                ProjectId = id,
                MessageId = messageId
            }));
        }

        [Authorize]
        [HttpPost]
        public ActionResult<BaseResponse<EmptyViewModel>> CreateMessage([FromBody] CreateDiscussionMessageRequestViewModel request)
        {
            return GetResult(GetCommand<CreateDiscussionMessageCommand>().Execute(request));
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> DeleteMessage([FromRoute] int id)
        {
            return GetResult(GetCommand<DeleteDiscussionMessageCommand>().Execute(new GenericRequestViewModel<int>
            {
                Data = id
            }));
        }
        
        [Authorize]
        [HttpGet]
        [Route("notifications/{id:int}")]
        public ActionResult<BaseResponse<IList<NotificationDto>>> GetNotifications([FromRoute] int id)
        {
            return GetResult(GetCommand<GetNotificationsCommand>().Execute(new GenericRequestViewModel<int>
            {
                Data = id
            }));
        }
        
        [Authorize]
        [HttpPost]
        [Route("comments")]
        public ActionResult<BaseResponse<EmptyViewModel>> PostComment([FromBody] PostCommentRequestViewModel request)
        {
            return GetResult(GetCommand<CreateCommentCommand>().Execute(request));
        }

        [Authorize]
        [HttpGet]
        [Route("comments/{id:int}")]
        public ActionResult<BaseResponse<IList<CommentDto>>> GetComments([FromRoute] int id)
        {
            return GetResult(GetCommand<GetAssignmentCommentsCommand>().Execute(new AssignmentInfoRequestViewModel
            {
                AssignmentId = id
            }));
        }
        
        [Authorize]
        [HttpDelete]
        [Route("comments/{id:int}")]
        public ActionResult<BaseResponse<EmptyViewModel>> DeleteComment([FromRoute] int id)
        {
            return GetResult(GetCommand<DeleteCommentCommand>().Execute(new GenericRequestViewModel<int>
            {
                Data = id
            }));
        }
    }
}