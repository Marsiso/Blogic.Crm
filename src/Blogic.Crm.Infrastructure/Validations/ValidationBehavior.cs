using FluentValidation;
using MediatR;
using ValidationException = Blogic.Crm.Domain.Exceptions.ValidationException;

namespace Blogic.Crm.Infrastructure.Validations;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : class?
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
		// Check available validators
		if (!_validators.Any())
		{
			return await next();
		}

		// Model validation
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

		// Return an exception in case of invalid model
		if (validationFailures.Any())
		{
			throw new ValidationException("Object sent from the client is invalid.", validationFailures);
		}

		return await next();
	}
}