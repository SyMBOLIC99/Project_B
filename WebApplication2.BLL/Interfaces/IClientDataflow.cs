using System.Threading.Tasks;

namespace WebApplication2.BLL.Interfaces
{
    public  interface IClientDataflow
    {
        Task SendClientDataFlow(byte[] data);
    }
}
