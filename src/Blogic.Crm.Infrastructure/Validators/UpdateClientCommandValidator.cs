using Blogic.Crm.Infrastructure.Commands;
using FluentValidation;
using PhoneNumbers;
using static Blogic.Crm.Domain.Data.Entities.User;
using static Blogic.Crm.Infrastructure.Extensions.DateTimeExtensions;
using static Blogic.Crm.Infrastructure.Extensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

public sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
	public UpdateClientCommandValidator()
	{
		When(c => IsNotNullOrEmpty(c.GivenName), () =>
		{
			RuleFor(c => c.GivenName)
				.MaximumLength(GivenNameMaximumLength)
				.WithMessage($"Client's given name must be at most {GivenNameMaximumLength} characters long.");
		});
		
		When(c => IsNotNullOrEmpty(c.FamilyName), () =>
		{
			RuleFor(c => c.FamilyName)
				.MaximumLength(FamilyNameMaximumLength)
				.WithMessage($"Client's family name must be at most {FamilyNameMaximumLength} characters long.");
		});
		
		When(c => IsNotNullOrEmpty(c.Email), () =>
		{
			RuleFor(c => c.Email)
				.MaximumLength(EmailMaximumLength)
				.WithMessage($"Client's email address must be at most {EmailMaximumLength} characters long.");
			
			RuleFor(c => c.Email)
				.EmailAddress()
				.WithMessage($"Client's email address format is invalid.");
		});
		
		When(c => IsNotNullOrEmpty(c.Phone), () =>
		{
			RuleFor(c => c.Phone)
				.MaximumLength(PhoneMaximumLength)
				.WithMessage($"Client's phone number must be at most {PhoneMaximumLength} characters long.");
			
			RuleFor(c => c.Phone)
				.Must(IsPhoneNumber!)
				.WithMessage("Client's phone number format is invalid.");
		});
		
		When(c => IsNotNullOrEmpty(c.BirthNumber), () =>
		{
			RuleFor(c => c.BirthNumber)
				.MaximumLength(BirthNumberMaximumLength)
				.WithMessage($"Client's birth number must be at most {BirthNumberMaximumLength} characters long.");
			
			RuleFor(c => c.BirthNumber)
				.Must(IsBirthNumber!)
				.WithMessage("Client's birth number format is invalid.");
		});

		When(c => c.DateBorn.HasValue, () =>
		{
			RuleFor(c => c.DateBorn!.Value)
				.Must(IsLegalAge)
				.WithMessage($"Client must be at least {AgeMinimumValue} years old.");
		});
	}
	
	public static bool IsPhoneNumber(string phone)
	{
		return PhoneNumberUtil.IsViablePhoneNumber(phone);
	}
	
	public static bool IsBirthNumber(string birthNumber)
	{
		foreach (var c in birthNumber.AsSpan())
		{
			if (char.IsDigit(c))
			{
				continue;
			}

			return false;
		}

		return true;
	}
	
	public static bool IsLegalAge(DateTime dateBorn)
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime legalAgeLowerBoundary = RemoveYears(utcNow, AgeMinimumValue);
		return DateTime.Compare(dateBorn, legalAgeLowerBoundary) < 0;
	}
}