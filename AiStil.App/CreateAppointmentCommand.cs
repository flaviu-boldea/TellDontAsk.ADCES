
namespace AiStil.App;

public class AppointmentRequest
{
    public int ClientId { get; set; }
    public int StylistId { get; set; }
    public required Slot Slot { get; set; }
}

public class CreateAppointmentCommand(
    AppointmentRequest request, 
    ISlotsRepository slotsRepository, 
    IClientsRepository clientsRepository,
    IStylistsRepository stylistsRepository)
{
    private readonly AppointmentRequest request = request;
    private readonly ISlotsRepository slots = slotsRepository;
    private readonly IClientsRepository clients = clientsRepository;
    private readonly IStylistsRepository stylists = stylistsRepository;

    public Appointment Execute()
    {
        if (!slots.IsSlotAvailable(request.Slot))
        {
            throw new Exception("Slot busy");
        }

        Client client = clients.GetClient(request.ClientId);
        Stylist stylist = stylists.GetStylist(request.StylistId);

        decimal cost;
        if (client.Membership == "Standard")
        {
            cost = stylist.GetPrice();
        }
        else
        {
            cost = 0;
        }

        return new Appointment()
        {
            Stylist = stylist,
            ClientId = request.ClientId,
            Slot = request.Slot,
            Cost = cost
        };
    }
}
