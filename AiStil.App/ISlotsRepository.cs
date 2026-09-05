
namespace AiStil.App;

public interface ISlotsRepository
{
    IEnumerable<Slot> GetSlots();
    bool IsSlotAvailable(Slot slot);
}
