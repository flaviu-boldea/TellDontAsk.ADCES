using AiStil.App;

namespace AiStil.Tests;

internal class ServicesRepositoryStub : IServicesRepository
{
    readonly IList<Service> services =
    [
        new Service() {ServiceId = 200, Name = "Haircut", Price = 300},
        new Service() {ServiceId = 201, Name = "Hair Coloring", Price = 500}
    ];

    public Service GetService(int serviceId)
    {
        return services.Single(service => service.ServiceId == serviceId);
    }
}