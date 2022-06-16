using Confluent.Kafka;
using MessagePack;

namespace WebApplication2.BLL.Services
{
    public class MessagePackSerializer<T> : ISerializer<T>
    {

        public byte[] Serialize(T data, SerializationContext context)
        {
            return MessagePackSerializer.Serialize(data);
        }

    }
}
