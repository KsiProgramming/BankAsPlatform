namespace BankAsPlatform;

using System;
using System.Globalization;

public readonly struct PercentageRate
{
    private const int MaxTotalDigits = 11;
    private const int MaxFractionalDigits = 10;
    private const decimal MaxValue = 1000m;
    private const decimal MinValue = 0m;

    public decimal Value { get; }

    public PercentageRate(decimal value)
    {
        if (value < MinValue || value > MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The percentage rate must be between 0 and 1000.");
        }

        string valueString = value.ToString("0.####################", CultureInfo.InvariantCulture);
        int totalDigits = valueString.Replace(".", "").Length;
        int fractionalDigits = valueString.Contains(".") ? valueString.Split('.')[1].Length : 0;

        if (totalDigits > MaxTotalDigits)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The total number of digits (before and after the decimal) must not exceed 11.");
        }

        if (fractionalDigits > MaxFractionalDigits)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The number of fractional digits must not exceed 10.");
        }

        Value = decimal.Round(value, MaxFractionalDigits, MidpointRounding.AwayFromZero);
    }

    public static PercentageRate FromDecimal(decimal value)
    {
        return new PercentageRate(value);
    }

    public override string ToString()
    {
        return Value.ToString("0.####################", CultureInfo.InvariantCulture);
    }

    public static implicit operator decimal(PercentageRate rate)
    {
        return rate.Value;
    }

    public decimal Calculate(decimal baseValue)
    {
        return baseValue * Value / 100;
    }
}
