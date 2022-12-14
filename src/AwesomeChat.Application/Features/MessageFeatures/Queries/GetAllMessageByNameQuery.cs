using AutoMapper;
using AwesomeChat.Application.RabbitMq;
using AwesomeChat.Domain.Entities;
using AwesomeChat.Domain.ViewModels;
using AwesomeChat.Persistence.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Application.Features.MessageFeatures.Queries
{
    public class GetAllMessageByNameQuery : IRequest<IEnumerable<MessageViewModel>>
    {
        public string RoomName { get; set; }
        public class GetAllMessageQueryHandler : IRequestHandler<GetAllMessageByNameQuery, IEnumerable<MessageViewModel>>
        {

            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            private readonly ApplicationDbContext _context;
            public GetAllMessageQueryHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
            }
            public async Task<IEnumerable<MessageViewModel>> Handle(GetAllMessageByNameQuery query, CancellationToken cancellationToken)
            {

                var room = _context.Rooms.FirstOrDefault(r => r.RoomName == query.RoomName);
                if (room == null)
                    return null;

                var messages = _context.Messages.Where(m => m.ToRoomId == room.Id)
                    .Include(m => m.FromUser)
                    .Include(m => m.ToRoom)
                    .OrderByDescending(m => m.Timestamp)
                    .Take(20)
                    .AsEnumerable()
                    .Reverse()
                    .ToList();
               
                var messagesViewModel = _mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messages);
                _rabbitMqProcuder.SendQueueMessage(messagesViewModel, "get-all-message");
                return messagesViewModel;


            }
        }
    }
}
