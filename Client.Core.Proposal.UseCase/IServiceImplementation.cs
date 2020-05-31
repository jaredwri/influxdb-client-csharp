using System.Threading.Tasks;

namespace Client.Core.Proposal.UseCase
{
    public interface IServiceImplementation
    {
        Task Login();
        Task Logout();
    }

    public class ServiceImplementation : IServiceImplementation
    {
        public Task Login() => Task.CompletedTask;

        public async Task Logout() => await Task.Delay(1000);
    }
}
