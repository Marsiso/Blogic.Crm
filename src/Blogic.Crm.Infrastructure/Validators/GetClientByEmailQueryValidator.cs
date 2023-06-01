using Blogic.Crm.Infrastructure.Queries;
using FluentValidation;
using static Blogic.Crm.Domain.Data.Entities.User;
using static Blogic.Crm.Infrastructure.Extensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

public sealed class GetClientByEmailQueryValidator : AbstractValidator<GetClientByEmailQuery>
{
	public GetClientByEmailQueryValidator()
	{
		When(c => IsNotNullOrEmpty(c.Email), () =>
		{
			RuleFor(c => c.Email)
				.MaximumLength(EmailMaximumLength)
				.WithMessage($"Email address must be at most {EmailMaximumLength} characters long.");
			
			RuleFor(c => c.Email)
				.EmailAddress()
				.WithMessage($"Email address format is invalid.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.Email)
				.NotEmpty()
				.WithMessage("Email address is required.");
		});
	}
}