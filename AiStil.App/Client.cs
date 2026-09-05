
namespace AiStil.App;

public class Client
{
    public int ClientId { get; set; }
    public required string Membership { get; set; }

    private decimal CalculateCost(decimal baseCost)
    {
        return Membership == "Standard" ? baseCost : 0;
    }

    public Appointment BookAppointment(Stylist stylist, Service service, Slot slot)
    {
        Appointment appointment = stylist.MakeAppointment(slot, service);
        appointment.Cost = CalculateCost(appointment.Cost);
        appointment.ClientId = ClientId;
        return appointment;
    }
}
