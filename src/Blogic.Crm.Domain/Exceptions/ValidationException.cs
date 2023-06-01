namespace Blogic.Crm.Domain.Exceptions;

public sealed class ValidationException : Exception
{
	public ValidationException(string message, IDictionary<string, string[]> validationFailures)
	{
		Message = message;
		ValidationFailures = validationFailures;
	}
	public override string Message { get; }
	public IDictionary<string, string[]> ValidationFailures { get; }
}