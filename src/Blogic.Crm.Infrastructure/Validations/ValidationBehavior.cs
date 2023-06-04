using FluentValidation;
using MediatR;
using ValidationException = Blogic.Crm.Domain.Exceptions.ValidationException;

namespace Blogic.Crm.Infrastructure.Validations;

/// <summary>
/// Handles command and query model validation before their execution.
/// </summary>
/// <typeparam name="TRequest">Either a command or query to be processed by their respective handlers.</typeparam>
/// <typeparam name="TResponse">Either a command or query response type.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request,
	                                    RequestHandlerDelegate<TResponse> next,
	                                    CancellationToken cancellationToken)
	{
		// When there are none validators available for the given request move onto the next command/query handler.
		if (_validators.Any() is false)
		{
			return await next();
		}

		// Perform model validation using available validators, when validation failure for the given request occurs
		// then group them by the property name so each name could have any or multiple validation failures.
		var context = new ValidationContext<TRequest>(request);
		var validationFailures = _validators
		                         .Select(validator => validator.Validate(context))
		                         .SelectMany(validationResult => validationResult.Errors)
		                         .Where(validationFailure => validationFailure != null)
		                         .GroupBy(
			                         validationFailure => validationFailure.PropertyName,
			                         validationFailure => validationFailure.ErrorMessage,
			                         (propertyName, validationFailures) => new
			                         {
				                         Key = propertyName,
				                         Values = validationFailures.Distinct().ToArray()
			                         })
		                         .ToDictionary(property => property.Key,
		                                       validationFailures => validationFailures.Values);

		// When there are any model validation failures then throw a new validation exception.
		if (validationFailures.Any())
		{
			throw new ValidationException("Object sent from the client is invalid.", validationFailures);
		}

		// Move onto the next command/query handler.
		return await next();
	}
}