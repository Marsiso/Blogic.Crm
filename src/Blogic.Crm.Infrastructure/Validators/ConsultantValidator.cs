using static Blogic.Crm.Domain.Data.Entities.User;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;
using static Blogic.Crm.Infrastructure.TypeExtensions.DateTimeExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

/// <summary>
///     <see cref="Consultant" /> model validations.
/// </summary>
public sealed class ConsultantValidator : AbstractValidator<Consultant>
{
	public ConsultantValidator()
	{
		When(c => IsNotNullOrEmpty(c.GivenName), () =>
		{
			RuleFor(c => c.GivenName)
				.MaximumLength(GivenNameMaximumLength)
				.WithMessage($"Consultant's given name must be at most {GivenNameMaximumLength} characters long.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.GivenName)
				.NotEmpty()
				.WithMessage("Consultant's given name is required.");
		});

		When(c => IsNotNullOrEmpty(c.FamilyName), () =>
		{
			RuleFor(c => c.FamilyName)
				.MaximumLength(FamilyNameMaximumLength)
				.WithMessage($"Consultant's family name must be at most {FamilyNameMaximumLength} characters long.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.FamilyName)
				.NotEmpty()
				.WithMessage("Consultant's family name is required.");
		});

		When(c => IsNotNullOrEmpty(c.Email), () =>
		{
			RuleFor(c => c.Email)
				.MaximumLength(EmailMaximumLength)
				.WithMessage($"Consultant's email address must be at most {EmailMaximumLength} characters long.");

			RuleFor(c => c.Email)
				.EmailAddress()
				.WithMessage("Consultant's email address format is invalid.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.Email)
				.NotEmpty()
				.WithMessage("Consultant's email address is required.");
		});

		When(c => IsNotNullOrEmpty(c.Phone), () =>
		{
			RuleFor(c => c.Phone)
				.MaximumLength(PhoneMaximumLength)
				.WithMessage($"Consultant's phone number must be at most {PhoneMaximumLength} characters long.");

			RuleFor(c => c.Phone)
				.Must(IsPhoneNumber)
				.WithMessage("Consultant's phone number format is invalid.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.Phone)
				.NotEmpty()
				.WithMessage("Consultant's phone number is required.");
		});

		When(c => IsNotNullOrEmpty(c.BirthNumber), () =>
		{
			RuleFor(c => c.BirthNumber)
				.MaximumLength(BirthNumberMaximumLength)
				.WithMessage($"Consultant's birth number must be at most {BirthNumberMaximumLength} characters long.");

			RuleFor(c => c.BirthNumber)
				.Matches(@"[0-9]{6}[/]?[0-9]{4}")
				.WithMessage("Consultant's birth number format is invalid.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.BirthNumber)
				.NotEmpty()
				.WithMessage("Consultant's birth number is required.");
		});

		When(c => IsNotNullOrEmpty(c.PasswordHash), () => { }).Otherwise(() =>
		{
			RuleFor(c => c.PasswordHash)
				.NotEmpty()
				.WithMessage("Password hash is required.");
		});

		RuleFor(c => c.DateBorn)
			.Must(db => IsLegalAge(db, AgeMinimumValue))
			.WithMessage($"Consultant must be at least {AgeMinimumValue} years old.");
	}
}