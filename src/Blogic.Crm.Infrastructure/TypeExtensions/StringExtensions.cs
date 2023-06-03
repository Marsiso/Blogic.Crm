using System.Diagnostics.CodeAnalysis;
using PhoneNumbers;

namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class StringExtensions
{
	public static bool IsNotNullOrEmpty([NotNullWhen(true)] string? value)
	{
		return !string.IsNullOrEmpty(value);
	}
	
	public static bool ContainSpecialCharacters([NotNullWhen(true)] string? value, int count)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}

		var matchCount = 0;
		foreach (var c in value.AsSpan())
		{
			if (char.IsSymbol(c))
			{
				matchCount++;
			}

			if (matchCount == count)
			{
				return true;
			}
		}

		return false;
	}
	
	public static bool ContainDigits([NotNullWhen(true)] string? value, int count)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}

		var matchCount = 0;
		foreach (var c in value.AsSpan())
		{
			if (char.IsDigit(c))
			{
				matchCount++;
			}

			if (matchCount == count)
			{
				return true;
			}
		}

		return false;
	}
	
	public static bool ContainLowerCaseCharacters([NotNullWhen(true)] string? value, int count)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}

		var matchCount = 0;
		foreach (var c in value.AsSpan())
		{
			if (char.IsLower(c))
			{
				matchCount++;
			}

			if (matchCount == count)
			{
				return true;
			}
		}

		return false;
	}
	
	public static bool ContainUpperCaseCharacters([NotNullWhen(true)] string? value, int count)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}

		var matchCount = 0;
		foreach (var c in value.AsSpan())
		{
			if (char.IsUpper(c))
			{
				matchCount++;
			}

			if (matchCount == count)
			{
				return true;
			}
		}

		return false;
	}
	
	public static bool IsBirthNumber(string? value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		
		foreach (var c in value.AsSpan())
		{
			if (char.IsDigit(c))
			{
				continue;
			}

			return false;
		}

		return true;
	}
	
	public static bool IsPhoneNumber(string? value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		
		return PhoneNumberUtil.IsViablePhoneNumber(value);
	}
}