using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Application.RabbitMq
{
    public class RabbitMqProcuder : IRabbitMqProcuder
    {

        private readonly IConfiguration _configuration;
        ConnectionFactory _connectionFactory = new ConnectionFactory();
        public RabbitMqProcuder(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration.GetValue<string>("RabbitMQ:HostName"),
                UserName = _configuration.GetValue<string>("RabbitMQ:UserName"),
                Password = _configuration.GetValue<string>("RabbitMQ:Password"),
                VirtualHost = _configuration.GetValue<string>("RabbitMQ:VirtualHost"),
            };
       
        }
        public void SendQueueMessage<T>(T @message,string @eventName)
        {
            IConnection connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare($"{@eventName}-queue", exclusive: false);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: $"{@eventName}-queue", body: body);
        }
    }
}
