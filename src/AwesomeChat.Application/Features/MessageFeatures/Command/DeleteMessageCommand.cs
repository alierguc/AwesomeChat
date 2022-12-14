using AutoMapper;
using AwesomeChat.Application.Features.MessageFeatures.Queries;
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
using System.Threading.Tasks;

namespace AwesomeChat.Application.Features.MessageFeatures.Command
{
    public class DeleteMessageCommand : IRequest<bool>
    {
        public Guid MessageId { get; set; }

        public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            private readonly IHubContext<AwesomeChatHub> _hubContext;
            public DeleteMessageCommandHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder, IHubContext<AwesomeChatHub> hubContext)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
                _hubContext = hubContext;
            }
            public async Task<bool> Handle(DeleteMessageCommand command, CancellationToken cancellationToken)
            {

             var message = await _context.Messages
             .Where(m => m.Id == command.MessageId).FirstOrDefaultAsync();

                if (message == null)
                    return false;

                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("removeChatMessage", message.Id);

                return true;
            }
        }
    }
}
