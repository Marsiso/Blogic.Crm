using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.TypeExtensions;
using FluentValidation;

namespace Blogic.Crm.Infrastructure.Validators;

/// <summary>
/// <see cref="Contract"/> model validations.
/// </summary>
public sealed class ContractValidator : AbstractValidator<Contract>
{
	public ContractValidator()
	{
		When(c => StringExtensions.IsNotNullOrEmpty(c.RegistrationNumber), () =>
		{
			RuleFor(c => c.RegistrationNumber)
				.Must(rn => Guid.TryParse(rn, out _))
				.WithMessage("The registration number is in the wrong format.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.RegistrationNumber)
				.NotEmpty()
				.WithMessage("A registration number is required.");
		});

		RuleFor(c => c.DateConcluded)
			.LessThanOrEqualTo(c => c.DateValid)
			.WithMessage("The date of conclusion of the contract must be the same or earlier than the validity date.");
		
		RuleFor(c => c.DateValid)
			.LessThanOrEqualTo(c => c.DateExpired)
			.WithMessage("The date of validity of the contract must be the same or earlier than the expiry date.");
	}
}