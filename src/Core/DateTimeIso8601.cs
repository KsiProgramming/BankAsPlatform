namespace BankAsPlatform;

using System;
using System.Globalization;

public readonly record struct DateTimeIso8601
{
    public DateTimeOffset Value { get; }

    // Constructor to initialize the value with validation
    private DateTimeIso8601(DateTimeOffset dateTimeValue)
    {
        Value = dateTimeValue;
    }

    // Factory method to create DateTimeIso8601 from DateTimeOffset
    public static DateTimeIso8601 FromDateTime(DateTimeOffset dateTimeValue)
    {
        ValidateOffset(dateTimeValue);
        ValidateFractionalSeconds(dateTimeValue);

        // Handle "24:00:00" edge case, converting it to the beginning of the next day.
        if (dateTimeValue.Hour == 24 && dateTimeValue.Minute == 0 && dateTimeValue.Second == 0)
        {
            dateTimeValue = dateTimeValue.AddDays(1).Date; // Set to the start of the next day
        }

        return new DateTimeIso8601(dateTimeValue);
    }

    // Parse method to handle ISO 8601 formats
    public static DateTimeIso8601 Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be null or empty", nameof(input));
        }

        // Parse input string to DateTimeOffset, considering the various ISO 8601 formats
        var dateTime = DateTimeOffset.ParseExact(input,
            [
                    "yyyy-MM-ddTHH:mm:ss.fffZ", // UTC format with Z
                    "yyyy-MM-ddTHH:mm:ss.fffK",  // Local time with UTC offset (+/-hh:mm)
                    "yyyy-MM-ddTHH:mm:ss.fff"    // Local time with no offset
            ],
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);

        return FromDateTime(dateTime);
    }

    // Validate that the offset is within the valid range of -14:00 to +14:00
    private static void ValidateOffset(DateTimeOffset dateTimeValue)
    {
        var offset = dateTimeValue.Offset;

        // ISO 8601 allows offsets between -14:00 and +14:00
        if (offset < TimeSpan.FromHours(-14) || offset > TimeSpan.FromHours(14))
        {
            throw new ArgumentOutOfRangeException(nameof(dateTimeValue.Offset),
                "The time zone offset must be between -14:00 and +14:00.");
        }
    }

    // Validate fractional seconds: Limiting to 3 digits for simplicity
    private static void ValidateFractionalSeconds(DateTimeOffset dateTimeValue)
    {
        // We use a custom format string to inspect the fractional seconds
        string formattedValue = dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture);

        // Check if the fractional seconds part exceeds 3 digits (milliseconds)
        var fractionalPart = formattedValue.Split('.')[1].TrimEnd('0');
        if (fractionalPart.Length > 3)
        {
            throw new ArgumentException("Fractional seconds cannot exceed 3 digits.", nameof(dateTimeValue));
        }
    }

    // Optional: Add a ToString method to represent the DateTimeIso8601 object as an ISO 8601 string
    public override string ToString()
    {
        // Use 3 digits for fractional seconds, and output in full ISO 8601 format
        return Value.ToString("yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture);
    }
}
