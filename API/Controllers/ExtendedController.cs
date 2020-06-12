using System.Net;
using DataAccess.Entities.User;
using DataAccess.Enums;
using Logic.Base;
using Logic.CommandFactory;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ExtendedController : ControllerBase
    {
        protected readonly ICommandFactory commandFactory;

        public ExtendedController(ICommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public string CurrentUserId => User.FindFirst("Id")?.Value;

        public ApplicationUser CurrentApplicationUser => HttpContext.Items["user"] as ApplicationUser;

        public TCommand GetCommand<TCommand>() where TCommand : class
        {
            return commandFactory.ResolveCommand<TCommand>(CurrentApplicationUser);
        }

        public ActionResult<BaseResponse<TResponse>> GetResult<TResponse>(BaseResponse<TResponse> response) where TResponse : class
        {
            switch (response.ResolveStatusCode())
            {
                case ResponseStatusCodes.Ok:
                    return Ok(response);
                case ResponseStatusCodes.Forbidden:
                    return StatusCode((int)HttpStatusCode.Forbidden, response);
                case ResponseStatusCodes.Unauthorized:
                    return Unauthorized(response);
                case ResponseStatusCodes.BadRequest:
                    return BadRequest(response);
                case ResponseStatusCodes.Created:
                    return Created("", response);
                case ResponseStatusCodes.NoContent:
                    return StatusCode((int)HttpStatusCode.NoContent, response);
                case ResponseStatusCodes.NotFound:
                    return NotFound(response);
                case ResponseStatusCodes.Accepted:
                    return Accepted(response);
                default:
                    return BadRequest();
            }
        }
    }
}