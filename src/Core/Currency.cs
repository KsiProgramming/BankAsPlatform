namespace BankAsPlatform;

// WIP
// Explicit and compliant with ISO 4217
public sealed record Currency
{
    private Currency(string AlphabeticCode, int NumericCode, int MinorUnit, string Entity) { }

    public readonly static Currency USD = new ("USD", 840, 2, "United States of America");

    public readonly static Currency EUR = new ("EUR", 978, 2, "European Union");

    public readonly static Currency JPY = new("JPY", 392, 0, "Japan");
}
