using ValidationException = Blogic.Crm.Domain.Exceptions.ValidationException;

namespace Blogic.Crm.Infrastructure.Validations;

/// <summary>
///     Handles the command and query model validation before their execution by their respective handlers.
/// </summary>
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
        // When there are none validators available for the given request move onto the command/query handler.
        if (_validators.Any() is false) return await next();

        // Perform the model validation using available validators, when there are any validation failures
        // for the given request then group them by the property name.
        // Each property could have zero or many validation failures.
        var context = new ValidationContext<TRequest>(request);
        var validationFailures = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
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
            throw new ValidationException("Object sent from the client is invalid.", validationFailures);

        // Move onto the next command/query handler.
        return await next();
    }
}