using AutoMapper;
using AwesomeChat.Application.Features.MessageFeatures.Queries;
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

namespace AwesomeChat.Application.Features.RoomFeatures.Queries
{
    public class GetAllRoomByNameQuery : IRequest<IEnumerable<RoomViewModel>>
    {
        public string RoomName { get; set; }
        public class GetAllRoomQueryHandler : IRequestHandler<GetAllRoomByNameQuery, IEnumerable<RoomViewModel>>
        {

            private readonly IMapper _mapper;
            private readonly IRabbitMqProcuder _rabbitMqProcuder;
            private readonly ApplicationDbContext _context;
            public GetAllRoomQueryHandler(ApplicationDbContext context, IMapper mapper, IRabbitMqProcuder rabbitMqProcuder)
            {
                _context = context;
                _mapper = mapper;
                _rabbitMqProcuder = rabbitMqProcuder;
            }
            public async Task<IEnumerable<RoomViewModel>> Handle(GetAllRoomByNameQuery query, CancellationToken cancellationToken)
            {

                var room = await _context.Rooms.Where(x=>x.isActive == true && x.RoomName == query.RoomName).FirstOrDefaultAsync();
                if (room == null)
                    return null; 

                var roomViewModel = _mapper.Map<Room, IEnumerable<RoomViewModel>>(room);
                _rabbitMqProcuder.SendQueueMessage(roomViewModel, "get-all-room-by-name");
                return roomViewModel;


            }
        }
    }
}
