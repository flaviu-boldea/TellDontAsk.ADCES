
namespace AiStil.App;

public class Stylist
{
    public int StylistId { get; set; }
    public required string Name { get; set; }

    public decimal GetPrice()
    {
        return 300;
    }
}
