using AutoMapper;
using AwesomeChat.Application.Hubs;
using AwesomeChat.Application.RabbitMq;
using AwesomeChat.Domain.Entities;
using AwesomeChat.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Application.Features.RoomFeatures.Command
{
    public class UpdateRoomCommand : IRequest<int>
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public string UserName { get; set; }
        public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            private readonly IHubContext<AwesomeChatHub> _hubContext;
            public UpdateRoomCommandHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder, IHubContext<AwesomeChatHub> hubContext)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
                _hubContext = hubContext;
            }
            public async Task<int> Handle(UpdateRoomCommand command, CancellationToken cancellationToken)
            {

                if (_context.Rooms.Any(r => r.RoomName == command.RoomName))
                    return 204;

                var room = await _context.Rooms
                    .Where(r => r.Id == command.RoomId && r.Admin.UserName == command.UserName)
                    .FirstOrDefaultAsync();

                if (room == null)
                    return 204;
                _rabbitMqProcuder.SendQueueMessage(room, "update-room");
                await _context.SaveChangesAsync(); 
                await _hubContext.Clients.All.SendAsync("updateChatRoom", new { id = room.Id, room.RoomName });

                return 200;
            }

        }
    }
}
