using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Domain.Dto.Message
{
    public class SendMessage
    {
        public string MessageToUsername { get; set; }
        public string MessageToRoomName { get; set; }
        public string MessageContent { get; set; }
    }
}
