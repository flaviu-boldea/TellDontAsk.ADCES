
namespace AiStil.App;

public class Stylist
{
    public int StylistId { get; set; }
    public required string Name { get; set; }
    public IList<int> QualifiedServiceIds { get; set; } = [];

    public Appointment MakeAppointment(Slot slot, Service service)
    {
        CanPerformService(service);
        
        return new Appointment{
            Slot = slot,
            Stylist = this,
            Service = service,
            Cost = service.Price
        };
    }

    public void CanPerformService(Service service)
    {
        if (!QualifiedServiceIds.Contains(service.ServiceId))
        {
            throw new Exception("Stylist not qualified for service");
        }
    }
}
