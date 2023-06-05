namespace Blogic.Crm.Domain.Exceptions;

/// <summary>
///     Exception that occurs during data model validation failures.
/// </summary>
public sealed class ValidationException : Exception
{
	public ValidationException(string message, IDictionary<string, string[]> validationFailures)
	{
		Message = message;
		ValidationFailures = validationFailures;
	}

	/// <summary>
	///     Validation exception description.
	/// </summary>
	public override string Message { get; }

	/// <summary>
	///     Dictionary that contains validation failures grouped by data model properties.
	/// </summary>
	public IDictionary<string, string[]> ValidationFailures { get; }
}