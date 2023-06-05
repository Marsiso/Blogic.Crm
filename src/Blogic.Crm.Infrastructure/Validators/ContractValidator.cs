using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

/// <summary>
///     <see cref="Contract" /> model validations.
/// </summary>
public sealed class ContractValidator : AbstractValidator<Contract>
{
	public ContractValidator()
	{
		When(c => IsNotNullOrEmpty(c.RegistrationNumber), () =>
		{
			RuleFor(c => c.RegistrationNumber)
				.Must(HasValidFormat)
				.WithMessage("The registration number is in the wrong format.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.RegistrationNumber)
				.NotEmpty()
				.WithMessage("A registration number is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.Institution), () =>
		{
		}).Otherwise(() =>
		{
			RuleFor(c => c.Institution)
				.NotEmpty()
				.WithMessage("Institution is required.");
		});

		RuleFor(c => c.DateConcluded)
			.LessThanOrEqualTo(c => c.DateValid)
			.WithMessage("The date of conclusion of the contract must be the same or earlier than the validity date.");

		RuleFor(c => c.DateValid)
			.LessThanOrEqualTo(c => c.DateExpired)
			.WithMessage("The date of validity of the contract must be the same or earlier than the expiry date.");
	}
	
	private static bool HasValidFormat(string registrationNumber)
	{
		return Guid.TryParse(registrationNumber, out _);
	}
}