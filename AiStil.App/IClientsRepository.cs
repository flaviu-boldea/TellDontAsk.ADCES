
namespace AiStil.App;

public interface IClientsRepository
{
    IEnumerable<Client> GetClients();

    Client GetClient(int clientId);
}
