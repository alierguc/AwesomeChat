using AwesomeChat.Application.Features.MessageFeatures.Command;
using AwesomeChat.Application.Features.MessageFeatures.Queries;
using AwesomeChat.Application.Features.RoomFeatures.Command;
using AwesomeChat.Application.Features.RoomFeatures.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeChat.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RoomController : BaseApiController
    {


        [MapToApiVersion("1.0")]
        [HttpGet("Room/GetAllRoomById/{roomId}")]
        public async Task<IActionResult> GetAllRoomById(Guid RoomId)
        {
            return Ok(await Mediator.Send(new GetRoomByIdQuery { Id = RoomId }));
        }

        [MapToApiVersion("1.0")]
        [HttpGet("Room/GetAllRoomById/{RoomName}")]
        public async Task<IActionResult> GetAllRoomByName(string RoomName)
        {
            return Ok(await Mediator.Send(new GetAllRoomByNameQuery { RoomName = RoomName }));
        }

        [MapToApiVersion("1.0")]
        [HttpPost("Room/CreateRoom")]
        public async Task<IActionResult> CreateRoom(CreateRoomCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [MapToApiVersion("1.0")]
        [HttpPut("Room/EditRoom/{roomId}")]
        public async Task<IActionResult> EditRoom(UpdateRoomCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("Room/DeleteRoom/{roomId}/{userName}")]
        public async Task<IActionResult> DeleteRoom(Guid roomId,string userName)
        {
            return Ok(await Mediator.Send(new DeleteRoomCommand { RoomId = roomId,UserName = userName }));
        }
    }
}
