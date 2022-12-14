using AutoMapper;
using AwesomeChat.Application.RabbitMq;
using AwesomeChat.Domain.Entities;
using AwesomeChat.Domain.ViewModels;
using AwesomeChat.Persistence.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Application.Features.MessageFeatures.Queries
{
    public class GetAllMessageByIdQuery : IRequest<MessageViewModel>
    {
        public Guid Id { get; set; }
        public class GetAllMessageByIdQueryHandler : IRequestHandler<GetAllMessageByIdQuery, MessageViewModel>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            public GetAllMessageByIdQueryHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
            }
            public async Task<MessageViewModel> Handle(GetAllMessageByIdQuery query, CancellationToken cancellationToken)
            {
                var messages = _context.Messages.Where(a => a.Id == query.Id).FirstOrDefault();
                if (messages == null) return null;
                var messagesViewModel = _mapper.Map<Message, MessageViewModel>(messages);
                _rabbitMqProcuder.SendQueueMessage(messagesViewModel, "get-all-message-by-id");
                return messagesViewModel;
            }
        }
    }
}
