using AutoMapper;
using AwesomeChat.Application.Features.MessageFeatures.Queries;
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

namespace AwesomeChat.Application.Features.RoomFeatures.Queries
{
    public class GetRoomByIdQuery : IRequest<RoomViewModel>
    {
        public Guid Id { get; set; }
        public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomViewModel>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            public GetRoomByIdQueryHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
            }
            public async Task<RoomViewModel> Handle(GetRoomByIdQuery query, CancellationToken cancellationToken)
            {
                var room = _context.Rooms.Where(a => a.Id == query.Id).FirstOrDefault();
                if (room == null) return null;
                var roomViewModel = _mapper.Map<Room, RoomViewModel>(room);
                _rabbitMqProcuder.SendQueueMessage(roomViewModel, "get-room-by-id");
                return roomViewModel;
            }
        }
    }
}
