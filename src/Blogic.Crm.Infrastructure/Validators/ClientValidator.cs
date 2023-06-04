using Blogic.Crm.Domain.Data.Entities;
using FluentValidation;
using static Blogic.Crm.Domain.Data.Entities.User;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;
using static Blogic.Crm.Infrastructure.TypeExtensions.DateTimeExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

public sealed class ClientValidator :AbstractValidator<Client>
{
	public ClientValidator()
	{
				When(c => IsNotNullOrEmpty(c.GivenName), () =>
		{
			RuleFor(c => c.GivenName)
				.MaximumLength(GivenNameMaximumLength)
				.WithMessage($"Client's given name must be at most {GivenNameMaximumLength} characters long.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.GivenName)
				.NotEmpty()
				.WithMessage("Client's given name is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.FamilyName), () =>
		{
			RuleFor(c => c.FamilyName)
				.MaximumLength(FamilyNameMaximumLength)
				.WithMessage($"Client's family name must be at most {FamilyNameMaximumLength} characters long.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.FamilyName)
				.NotEmpty()
				.WithMessage("Client's family name is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.Email), () =>
		{
			RuleFor(c => c.Email)
				.MaximumLength(EmailMaximumLength)
				.WithMessage($"Client's email address must be at most {EmailMaximumLength} characters long.");
			
			RuleFor(c => c.Email)
				.EmailAddress()
				.WithMessage($"Client's email address format is invalid.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.Email)
				.NotEmpty()
				.WithMessage("Client's email address is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.Phone), () =>
		{
			RuleFor(c => c.Phone)
				.MaximumLength(PhoneMaximumLength)
				.WithMessage($"Client's phone number must be at most {PhoneMaximumLength} characters long.");
			
			RuleFor(c => c.Phone)
				.Must(IsPhoneNumber)
				.WithMessage("Client's phone number format is invalid.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.Phone)
				.NotEmpty()
				.WithMessage("Client's phone number is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.BirthNumber), () =>
		{
			RuleFor(c => c.BirthNumber)
				.MaximumLength(BirthNumberMaximumLength)
				.WithMessage($"Client's birth number must be at most {BirthNumberMaximumLength} characters long.");
			
			RuleFor(c => c.BirthNumber)
				.Must(IsBirthNumber)
				.WithMessage("Client's birth number format is invalid.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.BirthNumber)
				.NotEmpty()
				.WithMessage("Client's birth number is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.PasswordHash), () =>
		{
		}).Otherwise(() =>
		{
			RuleFor(c => c.PasswordHash)
				.NotEmpty()
				.WithMessage("Password hash is required.");
		});
		
		RuleFor(c => c.DateBorn)
			.Must(db => IsLegalAge(db, User.AgeMinimumValue))
			.WithMessage($"Client must be at least {AgeMinimumValue} years old.");
	}
}