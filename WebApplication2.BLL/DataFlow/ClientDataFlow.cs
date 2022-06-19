using MessagePack;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using WebApplication2.BLL.Interfaces;

namespace WebApplication2.BLL.DataFlow
{
    public class ClientDataFlow : IClientDataflow


    { 
        private IKafkaService _kafkaproducer;
        private readonly Random RND = new Random();

        TransformBlock<byte[], Client> entryBlock = new TransformBlock<byte[], Client>(data => MessagePackSerializer.Deserialize<Client>(data));
        public ClientDataFlow(IKafkaService kafkaproducer)
        {

            _kafkaproducer = kafkaproducer;
        
        var enrichBlock = new TransformBlock<Client, Client>(c =>
        {
            c.Id = RND.Next(0, 50);
            return c;
        });

        var publishBlock = new ActionBlock<Client>(client =>
        {
            Console.WriteLine($"Updated Id Value: {client.Id}");
            _kafkaproducer.SendClientToKafka(client);

        });
        var linkOptions = new DataflowLinkOptions()
        {
            PropagateCompletion = true
        };

        entryBlock.LinkTo(enrichBlock, linkOptions);
            enrichBlock.LinkTo(publishBlock, linkOptions);
        }


    public async Task SendClientDataFlow(byte[] data)
    {
        var obj = MessagePackSerializer.Deserialize<Client>(data);
        Console.WriteLine($"Original Id Value: {obj.Id}");
        await entryBlock.SendAsync(data);
    }
}
}
