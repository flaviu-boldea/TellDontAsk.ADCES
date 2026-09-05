
using AiStil.App;

namespace AiStil.Tests;

internal class StylistsRepositoryStub : IStylistsRepository
{
    readonly IList<Stylist> stylists =
    [
        new Stylist() {StylistId = 100, Name = "Alice Stylist", QualifiedServiceIds = [200, 201]},
        new Stylist() {StylistId = 101, Name = "Bob Stylist", QualifiedServiceIds = [200]}
    ];

    public Stylist GetStylist(int stylistId)
    {
        return stylists.Single(p => p.StylistId == stylistId);
    }
}
