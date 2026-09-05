# Tell, Don’t Ask demo guide

This is the guided sequence we used to move the appointment logic toward Tell, Don’t Ask and better domain encapsulation.

## 1. Start with the command doing too much

The command was checking business rules directly:

- slot availability
- stylist qualification
- pricing logic

That is a smell because the command becomes a procedural orchestrator instead of a domain coordinator.

## 2. Move slot availability into the stylist

The stylist owns its own booked slots, so it should decide whether a slot is available.

Example:

```csharp
if (BookedSlots.Any(existingSlot =>
    existingSlot.Start < slot.End && slot.Start < existingSlot.End))
{
    throw new Exception("Slot busy");
}
```

This moves the rule to the object that owns the data.

## 3. Move service qualification into the stylist

The stylist should answer the question: can I perform this service?

Instead of external code reading `QualifiedServiceIds`, the stylist owns that rule.

Example:

```csharp
public void CanPerformService(Service service)
{
    if (!QualifiedServiceIds.Contains(service.ServiceId))
    {
        throw new Exception("Stylist not qualified for service");
    }
}
```

## 4. Keep the command as a coordinator

The command should:

- fetch the client
- fetch the stylist
- fetch the service
- delegate to the domain objects
- return the result

It should not be the place that performs most business validation.

## 5. Move pricing logic into the client

The client owns its membership information, so it should also own the pricing behavior.

Example:

```csharp
public decimal PriceFor(Service service)
{
    return Membership == "Standard" ? service.Price : 0;
}
```

This is better than external code reading the membership flag and deciding the discount itself.

## 6. Prefer a public domain behavior over a private helper

This is cleaner than exposing a low-level calculation method directly.

Prefer:

```csharp
client.PriceFor(service)
```

instead of:

```csharp
client.CalculateCost(service.Price)
```

The public method expresses the business behavior, not the implementation detail.

## 7. Use the value already produced by the domain object

If the stylist sets a base price, then the client should apply the membership rule to that price rather than re-deriving from the service blindly.

This keeps the domain flow consistent:

- stylist: base pricing / scheduling eligibility
- client: membership discount
- command: orchestration only

## 8. Keep the rule readable for the demo

For a teaching demo, this explicit overlap check is perfectly fine:

```csharp
existingSlot.Start < slot.End && slot.Start < existingSlot.End
```

It is simple and easy to explain. You can refactor it later into a helper method or a richer value object if needed.

## 9. The key message of the demo

The business rule should live with the object that owns the data.

That means:

- `Stylist` owns appointment availability and qualification
- `Client` owns pricing rules
- `CreateAppointmentCommand` orchestrates the workflow

This is the essence of Tell, Don’t Ask.

## 10. Refactor later when the concept is clear

After the audience understands the rule, you can improve the code by extracting:

- `Slot.Overlaps(...)`
- `StylistSchedule`
- richer appointment creation methods
- clearer domain methods

The goal of the first pass is clarity, not maximum abstraction.
