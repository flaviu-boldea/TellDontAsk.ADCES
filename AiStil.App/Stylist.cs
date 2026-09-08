
namespace AiStil.App;

public class Stylist
{
    public int StylistId { get; set; }
    public required string Name { get; set; }
    public IList<int> QualifiedServiceIds { private get; set; } = [];
    public IList<Slot> BookedSlots { private get; set; } = [];

    public Appointment MakeAppointment(Slot slot, Service service)
    {
        CanPerformService(service);
        EnsureSlotIsAvailable(slot);

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

    public void EnsureSlotIsAvailable(Slot slot)
    {
        if (BookedSlots.Any(existingSlot => existingSlot.Overlaps(slot)))
        {
            throw new Exception("Slot busy");
        }
    }
}
