
namespace AiStil.App;

public class Client
{
    public int ClientId { get; set; }
    public required string Membership { get; set; }

    public decimal CalculateCost(decimal baseCost)
    {
        return Membership == "Standard" ? baseCost : 0;
    }

    public decimal PriceFor(Service service)
    {
        return CalculateCost(service.Price);
    }

    public Appointment BookAppointment(Stylist stylist, Service service, Slot slot)
    {
        Appointment appointment = stylist.MakeAppointment(slot, service);
        appointment.Cost = PriceFor(service);
        appointment.ClientId = ClientId;
        return appointment;
    }
}
