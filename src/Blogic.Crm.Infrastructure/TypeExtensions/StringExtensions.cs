using System.Diagnostics.CodeAnalysis;

namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class StringExtensions
{
	public static bool IsNotNullOrEmpty([NotNullWhen(true)] string? value)
	{
		return !string.IsNullOrEmpty(value);
	}
}