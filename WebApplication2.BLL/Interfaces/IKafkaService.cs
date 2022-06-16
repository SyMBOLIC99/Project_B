using System.Threading.Tasks;

namespace WebApplication2.BLL.Interfaces
{
    public interface IKafkaService
    {
        Task SendClientToKafka(Client c);
    }
}
