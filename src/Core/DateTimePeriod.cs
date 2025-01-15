namespace BankAsPlatform;

public readonly record struct DateTimePeriod
{
    public DateTime Start { get; }

    public DateTime End { get; }

    private DateTimePeriod(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new ArgumentException($"Start date '{start}' must be earlier than or equal to end date '{end}'.");
        }

        this.Start = start;
        this.End = end;
    }

    public static DateTimePeriod Create(DateTime start, DateTime end)
        => new(start, end);

    public DateTimePeriod WithStartDateTime(DateTime start)
        => new(start, this.End);

    public DateTimePeriod WithEndDateTime(DateTime end)
        => new(this.Start, end);

    public bool HasNoEnd()
        => this.End == DateTime.MaxValue;

    public bool HasNoStart()
        => this.Start == DateTime.MinValue;

    public bool IsWithin(DateTime date)
        => date >= Start && date <= End;

    public bool Overlaps(DateTimePeriod other)
        => Start <= other.End && End >= other.Start;

    public override string ToString()
        => $"{Start:O} - {End:O}";
}
