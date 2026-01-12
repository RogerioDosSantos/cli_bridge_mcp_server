using System.Threading.Tasks;

namespace CliBridgeMCPServer.Interfaces
{
    public interface ITestService
    {
        Task SearchVideo(string videoSearch);
    }
}