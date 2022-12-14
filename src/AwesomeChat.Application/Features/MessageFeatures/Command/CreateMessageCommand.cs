using AutoMapper;
using AwesomeChat.Application.Hubs;
using AwesomeChat.Application.RabbitMq;
using AwesomeChat.Domain.Entities;
using AwesomeChat.Domain.ViewModels;
using AwesomeChat.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwesomeChat.Application.Features.MessageFeatures.Command
{
    public class CreateMessageCommand : IRequest<int>
    {

        public string MessageToUsername { get; set; }
        public string RoomName { get; set; }
        public string MessageFrom { get; set; }
        public string MessageContent { get; set; }
        public class CreateProductCommandHandler : IRequestHandler<CreateMessageCommand, int>
        {

            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            private readonly IHubContext<AwesomeChatHub> _hubContext;
           // private readonly IApplicationDbContext _context;
            public CreateProductCommandHandler(
                ApplicationDbContext context, 
                IMapper mapper, 
                IRabbitMqProcuder rabbitMqProcuder,
                IHubContext<AwesomeChatHub> hubContext)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
                _hubContext = hubContext;
            }
            public async Task<int> Handle(CreateMessageCommand command, CancellationToken cancellationToken)
            {

                    var user = _context.Users.Where(u => u.UserName == command.MessageToUsername).ToList();
                    if(user.Count != 0)
                    {
                      var room = _context.Rooms.FirstOrDefault(r => r.RoomName == command.RoomName);
                    if (room == null)
                    {
                        return 204;
                    }
                    else
                    {
                        var msg = new Message()
                        {
                            MessageContent = Regex.Replace(command.MessageContent, @"<.*?>", string.Empty),
                            FromUser = user.FirstOrDefault(),
                            ToRoom = room,
                            Timestamp = DateTime.Now
                        };

                        _context.Messages.Add(msg);
                        await _context.SaveChangesAsync();
                        var createdMessage = _mapper.Map<Message, MessageViewModel>(msg);
                        _rabbitMqProcuder.SendQueueMessage(createdMessage, "create-message");
                        await _hubContext.Clients.Group(room.RoomName).SendAsync("newMessage", createdMessage);
                        return 200;
                    }

                }
                else
                {
                    return 204;
                }         
            }
        }

    }
}
