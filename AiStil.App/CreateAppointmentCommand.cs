
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

        Appointment appointment = stylist.MakeAppointment(request.Slot, service);

        appointment.Cost = client.CalculateCost(appointment.Cost);
        appointment.ClientId = request.ClientId;
        return appointment;
    }
}
