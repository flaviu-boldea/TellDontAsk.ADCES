
using AiStil.App;

namespace AiStil.Tests;

public class CreateAppointmentTests
{
    readonly ISlotsRepository slotsRepo = new SlotsRepositoryStub();
    readonly IStylistsRepository stylistsRepo = new StylistsRepositoryStub();
    readonly IClientsRepository clientsRepo = new ClientsRepositoryStub();
    readonly IServicesRepository servicesRepo = new ServicesRepositoryStub();
    readonly IList<Client> clients;

    public CreateAppointmentTests()
    {
        clients = clientsRepo.GetClients().ToList();
    }

    int stylistId = 100;
    int serviceId = 200;

    [Fact]
    public void ShouldCreateAppointmentWhenSlotAvailable()
    {
        Slot emptySlot = new(new DateTime(2024, 10, 20, 8, 30, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            ServiceId = serviceId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo).Execute();

        Assert.NotNull(appointment);
        Assert.Equal(standardClientId, appointment.ClientId);
        Assert.Equal(stylistId, appointment.Stylist.StylistId);
        Assert.Equal(15, emptySlot.End.Subtract(emptySlot.Start).TotalMinutes);
    }

    [Fact]
    public void ShouldNotCreateAppointmentWhenSlotBusy()
    {
        Slot busySlot = new(new DateTime(2024, 10, 20, 8, 0, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            ServiceId = serviceId,
            Slot = busySlot
        };

        var command = new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo);

        var exception = Assert.Throws<Exception>(() => command.Execute());
        Assert.Equal("Slot busy", exception.Message);
    }

    [Fact]
    public void ShouldDetectWhenSlotsOverlap()
    {
        var first = new Slot(new DateTime(2024, 10, 20, 8, 0, 0), 15);
        var second = new Slot(new DateTime(2024, 10, 20, 8, 10, 0), 15);

        Assert.True(first.Overlaps(second));
    }

    [Fact]
    public void ShouldRejectBusySlotFromStylistSchedule()
    {
        var stylist = stylistsRepo.GetStylist(stylistId);
        stylist.BookedSlots =
        [
            new Slot(new DateTime(2024, 10, 20, 8, 0, 0), 15)
        ];

        Slot busySlot = new(new DateTime(2024, 10, 20, 8, 0, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            ServiceId = serviceId,
            Slot = busySlot
        };

        var command = new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo);

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
            ServiceId = serviceId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo).Execute();

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
            ServiceId = serviceId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo).Execute();

        Assert.NotNull(appointment);
        Assert.Equal(300, appointment.Cost);
    }

    [Fact]
    public void ShouldReturnPriceForSelectedService()
    {
        Slot emptySlot = new(new DateTime(2024, 10, 20, 8, 30, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = stylistId,
            ClientId = standardClientId,
            ServiceId = 201,
            Slot = emptySlot
        };
        Appointment appointment =
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo).Execute();

        Assert.Equal(500, appointment.Cost);
        Assert.Equal(201, appointment.Service.ServiceId);
    }

    [Fact]
    public void ShouldFailWhenStylistIsNotQualifiedForService()
    {
        Slot emptySlot = new(new DateTime(2024, 10, 20, 8, 30, 0), 15);
        int standardClientId = clients.First().ClientId;
        AppointmentRequest request = new()
        {
            StylistId = 101,
            ClientId = standardClientId,
            ServiceId = 201,
            Slot = emptySlot
        };

        var command = new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo);

        var exception = Assert.Throws<Exception>(() => command.Execute());
        Assert.Equal("Stylist not qualified for service", exception.Message);
    }

    [Fact]
    public void ClientShouldApplyMembershipDiscountToServicePrice()
    {
        var client = clients.Last();
        var service = servicesRepo.GetService(serviceId);

        var discountedPrice = client.CalculateCost(service.Price);

        Assert.Equal(0, discountedPrice);
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
            ServiceId = serviceId,
            Slot = emptySlot
        };
        Appointment appointment = 
            new CreateAppointmentCommand(request, slotsRepo, clientsRepo, stylistsRepo, servicesRepo).Execute();

        Assert.NotNull(appointment);
        Assert.Equal(0, appointment.Cost);
    }
}
