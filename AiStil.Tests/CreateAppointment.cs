
using AiStil.App;

namespace AiStil.Tests;

public class CreateAppointmentTests
{
    readonly ISlotsRepository slotsRepo = new SlotsRepositoryStub();
    readonly IStylistsRepository stylistsRepo = new StylistsRepositoryStub();
    readonly IClientsRepository clientsRepo = new ClientsRepositoryStub();
    readonly IList<Client> clients;

    public CreateAppointmentTests()
    {
        clients = clientsRepo.GetClients().ToList();
    }

    int stylistId = 100;

    [Fact]
    public void ShouldSucceedWhenSlotAvailable()
    {
        Slot emptySlot = new(new DateTime(2024, 10, 20, 8, 30, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo).Execute();

        Assert.NotNull(appointment);
        Assert.Equal(standardClientId, appointment.ClientId);
        Assert.Equal(stylistId, appointment.Stylist.StylistId);
        Assert.Equal(15, emptySlot.End.Subtract(emptySlot.Start).TotalMinutes);
    }

    [Fact]
    public void ShouldFailedWhenSlotBusy()
    {
        Slot busySlot = new(new DateTime(2024, 10, 20, 8, 0, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            Slot = busySlot
        };

        var command = new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo);

        var exception = Assert.Throws<Exception>(() => command.Execute());
        Assert.Equal("Slot busy", exception.Message);
    }

    [Fact]
    public void SouldReturnStylistDetails()
    {
        Slot emptySlot = new(new DateTime(2024, 10, 20, 8, 30, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo).Execute();

        Assert.NotNull(appointment);
        Assert.Equal(standardClientId, appointment.ClientId);
        Assert.Equal(stylistId, appointment.Stylist.StylistId);
        Assert.Equal("Alice Stylist", appointment.Stylist.Name);
        Assert.Equal(15, emptySlot.End.Subtract(emptySlot.Start).TotalMinutes);
    }

    [Fact]
    public void SouldReturnFullPriceWhenNotIncludedInMembership()
    {
        Slot emptySlot = new(new DateTime(2024, 10, 20, 8, 30, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo).Execute();

        Assert.NotNull(appointment);
        Assert.Equal(300, appointment.Cost);
    }

    [Fact]
    public void SouldReturnZeroPriceWhenIncludedInMembership()
    {
        Slot emptySlot = new(new DateTime(2024, 10, 20, 8, 30, 0), 15);
        int premiumClientId = clients.Last().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = premiumClientId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo).Execute();

        Assert.NotNull(appointment);
        Assert.Equal(0, appointment.Cost);
    }
}
