
using AiStil.App;

namespace AiStil.Tests;

internal class ClientsRepositoryStub : IClientsRepository
{
    readonly IList<Client> clients =
    [
        new Client() {ClientId = 501, Membership = "Standard"},
        new Client() {ClientId = 601, Membership = "Premium"}
    ];

    public Client GetClient(int clientId)
    {
        return clients.Single(p => p.ClientId == clientId);
    }

    public IEnumerable<Client> GetClients()
    {
        return clients;
    }
}
