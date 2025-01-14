namespace BankAsPlatform;

using System.Globalization;

public readonly record struct Amount
{
    private const int MaxTotalDigits = 18;
    private const int FractionalDigits = 5;
    private const decimal MinAmount = 0m;

    public decimal Value { get; }

    private Amount(decimal value)
    {
        this.Value = value;
    }

    public static Amount Create(decimal value)
    {
        if (value < MinAmount || value >= (decimal)Math.Pow(10, MaxTotalDigits))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Amount must be between 0 and 10^18.");
        }

        value = Math.Abs(value);

        value = decimal.Round(value, FractionalDigits, MidpointRounding.AwayFromZero);

        return new Amount(value);
    }

    public override string ToString()
    {
        return Value.ToString("0.#####", CultureInfo.InvariantCulture);
    }
}
