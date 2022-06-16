using Confluent.Kafka;
using System.Threading.Tasks;
using WebApplication2.BLL.Interfaces;

namespace WebApplication2.BLL.Services
{
    public class KafkaService : IKafkaService
    {
        private static IProducer<int, Client> _producer;
        public KafkaService()
        {

            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
            };
            _producer = new ProducerBuilder<int, Client>(config)
                .SetValueSerializer(new MessagePackSerializer<Client>())
                .Build();

        }
        public async Task SendClientToKafka(Client c)
        {
            await Task.Factory.StartNew(async () =>
            {
                await _producer.ProduceAsync("test_Client", new Message<int, Client>()
                {

                    Key = c.Id,
                    Value = c
                });
            });
    }
    }
}
