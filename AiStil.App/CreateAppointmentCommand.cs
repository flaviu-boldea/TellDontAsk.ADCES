
namespace AiStil.App;

public class AppointmentRequest
{
    public int ClientId { get; set; }
    public int StylistId { get; set; }
    public int ServiceId { get; set; }
    public required Slot Slot { get; set; }
}

public class CreateAppointmentCommand(
    AppointmentRequest request, 
    ISlotsRepository slotsRepository, 
    IClientsRepository clientsRepository,
    IStylistsRepository stylistsRepository,
    IServicesRepository servicesRepository)
{
    private readonly AppointmentRequest request = request;
    private readonly ISlotsRepository slots = slotsRepository;
    private readonly IClientsRepository clients = clientsRepository;
    private readonly IStylistsRepository stylists = stylistsRepository;
    private readonly IServicesRepository services = servicesRepository;

    public Appointment Execute()
    {
        if (!slots.IsSlotAvailable(request.Slot))
        {
            throw new Exception("Slot busy");
        }

        Client client = clients.GetClient(request.ClientId);
        Stylist stylist = stylists.GetStylist(request.StylistId);
        Service service = services.GetService(request.ServiceId);

        if (!stylist.QualifiedServiceIds.Contains(service.ServiceId))
        {
            throw new Exception("Stylist not qualified for service");
        }

        decimal cost;
        if (client.Membership == "Standard")
        {
            cost = service.Price;
        }
        else
        {
            cost = 0;
        }

        return new Appointment()
        {
            Stylist = stylist,
            Service = service,
            ClientId = request.ClientId,
            Slot = request.Slot,
            Cost = cost
        };
    }
}
