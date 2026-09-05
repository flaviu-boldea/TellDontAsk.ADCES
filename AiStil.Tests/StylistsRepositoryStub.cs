
using AiStil.App;

namespace AiStil.Tests;

internal class StylistsRepositoryStub : IStylistsRepository
{
    readonly IList<Stylist> stylists =
    [
        new Stylist()
        {
            StylistId = 100,
            Name = "Alice Stylist",
            QualifiedServiceIds = [200, 201],
            BookedSlots =
            [
                new Slot(new DateTime(2024, 10, 20, 8, 0, 0), 15),
                new Slot(new DateTime(2024, 10, 20, 8, 15, 0), 15),
                new Slot(new DateTime(2024, 10, 20, 9, 30, 0), 15)
            ]
        },
        new Stylist()
        {
            StylistId = 101,
            Name = "Bob Stylist",
            QualifiedServiceIds = [200],
            BookedSlots = []
        }
    ];

    public Stylist GetStylist(int stylistId)
    {
        return stylists.Single(p => p.StylistId == stylistId);
    }
}
