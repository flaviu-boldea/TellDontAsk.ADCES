
namespace AiStil.App;

public class Client
{
    public int ClientId { get; set; }
    public required string Membership { get; set; }

    public decimal CalculateCost(decimal baseCost)
    {
        return Membership == "Standard" ? baseCost : 0;
    }
}
