using AwesomeChat.Application.Features.MessageFeatures.Command;
using AwesomeChat.Application.Features.MessageFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace AwesomeChat.API.Controllers.v1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MessageController : BaseApiController
    {


        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
            _logger.LogInformation("MessageController Constructor");
        }

        /// <summary>
        /// SendChatMessage
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("Message/SendChatMessage")]
        public async Task<IActionResult> SendMessage(CreateMessageCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [MapToApiVersion("1.0")]
        [HttpGet("Message/GetMessageByRoomName/{roomName}")]
        public async Task<IActionResult> GetMessageByRoomName(string roomName)
        {
            return Ok(await Mediator.Send(new GetAllMessageByNameQuery { RoomName=roomName }));
        }


        /// <summary>
        /// GetTestMessage
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("Message/GetTestMessage")]
        public async Task<IActionResult> GetTestMessage()
        {
            _logger.LogInformation("GetTestMessage Message Log");
            return Ok("GetTestMessage Correct");
        }
        [MapToApiVersion("1.0")]
        [HttpGet("Message/GetMessageById/{messageId}")]
        public async Task<IActionResult> GetMessageById(Guid messageId)
        {
            return Ok(await Mediator.Send(new GetAllMessageByIdQuery { Id = messageId }));
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("Message/DeleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            return Ok(await Mediator.Send(new DeleteMessageCommand { MessageId = messageId }));
        }
    }
}
