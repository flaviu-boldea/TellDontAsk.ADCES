
using AiStil.App;

namespace AiStil.Tests;

internal class SlotsRepositoryStub : ISlotsRepository
{
    readonly IList<Slot> slots =
    [
        new Slot(new DateTime(2024, 10, 20, 8, 0, 0), 15),
        new Slot(new DateTime(2024, 10, 20, 8, 15, 0), 15),
        new Slot(new DateTime(2024, 10, 20, 9, 30, 0), 15)
    ];

    public IEnumerable<Slot> GetSlots()
    {
        return slots;
    }

    public bool IsSlotAvailable(Slot slot)
    {
        return !slots.Contains(slot);
    }
}
