public record Slot
{
    public DateTime Start { get; }
    public DateTime End { get; }
    public int Interval { get; }

    public Slot(DateTime start, int length)
    {
        Start = start;
        Interval = length;
        End = start.AddMinutes(length);
    }
}