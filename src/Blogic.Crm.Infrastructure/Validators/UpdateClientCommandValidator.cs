using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static Blogic.Crm.Domain.Data.Entities.User;
using static Blogic.Crm.Infrastructure.TypeExtensions.DateTimeExtensions;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

public sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
	private readonly DataContext _dataContext;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
	
	public UpdateClientCommandValidator(DataContext dataContext, IEmailLookupNormalizer emailLookupNormalizer,
	                                    IPhoneLookupNormalizer phoneLookupNormalizer)
	{
		_dataContext = dataContext;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;

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
			
			RuleFor(c => c.Email)
				.Must((c, e) => EmailNotTaken(c.Id, e))
				.WithMessage("Client's email address already taken.");
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
			
			RuleFor(c => c.Phone)
				.Must((c, p) => PhoneNotTaken(c.Id, p))
				.WithMessage("Client's phone number already taken.");
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
			
			RuleFor(c => c.BirthNumber)
				.Must((c, b) => BirthNumberNotTaken(c.Id, b))
				.WithMessage("Client's birth number already taken.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.BirthNumber)
				.NotEmpty()
				.WithMessage("Client's birth number is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.Password), () =>
		{
			RuleFor(c => c.Password)
				.Must(p => ContainSpecialCharacters(p, RequiredSpecialCharacters))
				.WithMessage($"Password must contain at least {RequiredSpecialCharacters} special characters.");
			
			RuleFor(c => c.Password)
				.Must(p => ContainDigits(p, RequiredDigitCharacters))
				.WithMessage($"Password must contain at least {RequiredDigitCharacters} special characters.");
			
			RuleFor(c => c.Password)
				.Must(p => ContainLowerCaseCharacters(p, RequiredLowerCaseCharacters))
				.WithMessage($"Password must contain at least {RequiredLowerCaseCharacters} lower case characters.");
			
			RuleFor(c => c.Password)
				.Must(p => ContainUpperCaseCharacters(p, RequiredUpperCaseCharacters))
				.WithMessage($"Password must contain at least {RequiredUpperCaseCharacters} upper case characters.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.Password)
				.NotEmpty()
				.WithMessage("Password is required.");
		});
		
		RuleFor(c => c.DateBorn)
			.Must(db => IsLegalAge(db, AgeMinimumValue))
			.WithMessage($"Client must be at least {AgeMinimumValue} years old.");
	}

	public bool EmailNotTaken(long id, string email)
	{
		string normalizedEmail = _emailLookupNormalizer.Normalize(email)!;
		Client? client = _dataContext.Clients.AsNoTracking().SingleOrDefault(c => c.NormalizedEmail == normalizedEmail);
		if (client == null)
		{
			return true;
		}

		return client.Id == id;
	}
	
	public bool PhoneNotTaken(long id, string phone)
	{
		string normalizedPhone = _phoneLookupNormalizer.Normalize(phone)!;
		Client? client = _dataContext.Clients.AsNoTracking().SingleOrDefault(c => c.Phone == normalizedPhone);
		if (client == null)
		{
			return true;
		}

		return client.Id == id;
	}

	public bool BirthNumberNotTaken(long id, string birthNumber)
	{
		Client? client = _dataContext.Clients.AsNoTracking().SingleOrDefault(c => c.BirthNumber == birthNumber);
		if (client == null)
		{
			return true;
		}

		return client.Id == id;
	}
}