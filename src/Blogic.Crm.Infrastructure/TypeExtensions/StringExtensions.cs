using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using PhoneNumbers;
using static System.Char;
using static System.String;

namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static partial class StringExtensions
{
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] string? value)
    {
        return !IsNullOrEmpty(value);
    }

    public static bool ContainSpecialCharacters([NotNullWhen(true)] string? value, int count)
    {
        if (IsNullOrEmpty(value)) return false;

        var matchCount = 0;
        foreach (var c in value.AsSpan())
        {
            if (IsSymbol(c)) matchCount++;

            if (matchCount == count) return true;
        }

        return false;
    }

    public static bool ContainDigits([NotNullWhen(true)] string? value, int count)
    {
        if (IsNullOrEmpty(value)) return false;

        var matchCount = 0;
        foreach (var c in value.AsSpan())
        {
            if (IsDigit(c)) matchCount++;

            if (matchCount == count) return true;
        }

        return false;
    }

    public static bool ContainLowerCaseCharacters([NotNullWhen(true)] string? value, int count)
    {
        if (IsNullOrEmpty(value)) return false;

        var matchCount = 0;
        foreach (var c in value.AsSpan())
        {
            if (IsLower(c)) matchCount++;

            if (matchCount == count) return true;
        }

        return false;
    }

    public static bool ContainUpperCaseCharacters([NotNullWhen(true)] string? value, int count)
    {
        if (IsNullOrEmpty(value)) return false;

        var matchCount = 0;
        foreach (var c in value.AsSpan())
        {
            if (IsUpper(c)) matchCount++;

            if (matchCount == count) return true;
        }

        return false;
    }

    public static bool IsBirthNumber([NotNullWhen(true)] string? value)
    {
        if (IsNullOrEmpty(value)) return false;

        foreach (var c in value.AsSpan())
        {
            if (IsDigit(c)) continue;

            return false;
        }

        return true;
    }

    public static bool IsPhoneNumber([NotNullWhen(true)] string? value)
    {
        return !IsNullOrEmpty(value) && PhoneNumberUtil.IsViablePhoneNumber(value);
    }

    public static bool EqualsNot(string? left, string? right, StringComparison comparisonType)
    {
        return !string.Equals(left, right, comparisonType);
    }

    public static string? FormatBirthNumber([NotNullIfNotNull(nameof(birthNumber))] string? birthNumber)
    {
        if (IsNullOrEmpty(birthNumber)) return birthNumber;

        var validFormat = BirthNumberRegularExpression().IsMatch(birthNumber);
        if (validFormat is false) return birthNumber;

        var span = birthNumber.AsSpan();
        var containsDelimiter = birthNumber[7].Equals('/');
        return containsDelimiter ? birthNumber : $"{span[..6]}/{span[6..]}";
    }

    [GeneratedRegex("[0-9]{6}[/]?[0-9]{4}", RegexOptions.Compiled)]
    private static partial Regex BirthNumberRegularExpression();
}