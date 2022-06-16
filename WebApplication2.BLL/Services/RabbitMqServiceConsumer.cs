using MessagePack;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApplication2.BLL.Interfaces;

namespace WebApplication2
{
    public class RabbitMqServiceConsumer : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IClientDataflow _clientdataflow;

        public RabbitMqServiceConsumer(IClientDataflow clientdataflow)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("Test", ExchangeType.Fanout, durable: true);
            _clientdataflow = clientdataflow;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {
                _channel.QueueDeclare("client_queue", true, false, autoDelete: false);
                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (sender, ea) =>
                {
                    Client c = MessagePackSerializer.Deserialize<Client>(ea.Body.ToArray());


                    Console.WriteLine($"Consumed from RabbitMQ : { c.Id}, { c.Name}");

                    _clientdataflow.SendClientDataFlow(ea.Body.ToArray());
                };
                _channel.BasicConsume(queue: "client_queue", autoAck: true, consumer: consumer);
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
    }
}
