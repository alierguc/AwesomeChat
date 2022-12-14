using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Application.RabbitMq
{
    public interface IRabbitMqProcuder
    {
        public void SendQueueMessage<T>(T @message,string @eventName);
    }
}
