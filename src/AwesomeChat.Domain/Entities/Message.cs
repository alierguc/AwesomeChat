using AwesomeChat.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }
        public ApplicationUser FromUser { get; set; }
        public Guid ToRoomId { get; set; }
        public Room ToRoom { get; set; }
    }
}
