using System.Diagnostics.CodeAnalysis;
using static System.IO.Path;
using static System.String;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class PathExtensions
{
	public const string CsvFileExtension = ".csv";

	public static bool PathDoesNotExist([NotNullWhen(false)] string? absolutePath)
	{
		return !Exists(absolutePath);
	}

	public static bool IsNotCsvFile([NotNullWhen(true)] string? fileNameWithExtension)
	{
		if (IsNullOrEmpty(fileNameWithExtension))
		{
			return true;
		}

		if (HasExtension(fileNameWithExtension))
		{
			var extension = GetExtension(fileNameWithExtension);
			return EqualsNot(extension, CsvFileExtension, StringComparison.Ordinal);
		}

		return true;
	}
}