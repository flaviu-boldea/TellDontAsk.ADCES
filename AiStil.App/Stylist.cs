
namespace AiStil.App;

public class Stylist
{
    public int StylistId { get; set; }
    public required string Name { get; set; }
    public IList<int> QualifiedServiceIds { get; set; } = [];
    public IList<Slot> BookedSlots { get; set; } = [];

    public bool IsAvailable(Slot slot)
    {
        return !BookedSlots.Any(existingSlot => existingSlot.Start < slot.End && slot.Start < existingSlot.End);
    }
}
