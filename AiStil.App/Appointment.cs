
namespace AiStil.App;

public class Appointment
{
    public int ClientId { get; set; }
    public required Stylist Stylist { get; set; }
    public required Service Service { get; set; }
    public required Slot Slot { get; set; }
    public decimal Cost { get; set; }
}
