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
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwesomeChat.Application.Features.RoomFeatures.Command
{
    public class DeleteRoomCommand : IRequest<bool>
    {
        public Guid RoomId { get; set; }
        public string UserName { get; set; }

        public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, bool>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            private readonly IHubContext<AwesomeChatHub> _hubContext;
            public DeleteRoomCommandHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder, IHubContext<AwesomeChatHub> hubContext)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
                _hubContext = hubContext;
            }
            public async Task<bool> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
            {
                try {
                    var room = await _context.Rooms
                   .Where(r => r.Id == command.RoomId && r.Admin.UserName == command.UserName)
                   .FirstOrDefaultAsync();

                    if (room == null)
                        return false;

                    _context.Rooms.Remove(room);
                    await _context.SaveChangesAsync();

                    _rabbitMqProcuder.SendQueueMessage(room, "remove-on-chat-room");
                    await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
                    await _hubContext.Clients.Group(room.RoomName).SendAsync("onRoomDeleted", string.Format("Room {0} has been deleted.\nYou are moved to the first available room!", room.RoomName));

                    return true;


                } catch
                {
                    return false;
                }
               
            }
            }
        }
    }

