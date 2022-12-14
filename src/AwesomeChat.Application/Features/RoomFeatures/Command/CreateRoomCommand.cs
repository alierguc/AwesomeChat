using AutoMapper;
using AwesomeChat.Application.Features.MessageFeatures.Command;
using AwesomeChat.Application.Hubs;
using AwesomeChat.Application.RabbitMq;
using AwesomeChat.Domain.Entities;
using AwesomeChat.Domain.ViewModels;
using AwesomeChat.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwesomeChat.Application.Features.RoomFeatures.Command
{
    public class CreateRoomCommand : IRequest<int>
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public string Name { get; set; }

        public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            private readonly IHubContext<AwesomeChatHub> _hubContext;
            public CreateRoomCommandHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder, IHubContext<AwesomeChatHub> hubContext)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
                _hubContext = hubContext;
            }
            public async Task<int> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
            {

                if (_context.Rooms.Any(r => r.RoomName == command.RoomName))
                    return 500;

                var user = _context.Users.FirstOrDefault(u => u.UserName == command.Name);
                var room = new Room()
                {
                    RoomName = command.RoomName,
                    Admin = user
                };

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                _rabbitMqProcuder.SendQueueMessage(room, "create-room");
                await _hubContext.Clients.All.SendAsync("addChatRoom", new { id = room.Id, name = room.RoomName }); 
                return 200;
             }
            
            }
        }
}
