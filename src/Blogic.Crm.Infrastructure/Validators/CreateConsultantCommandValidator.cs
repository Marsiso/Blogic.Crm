﻿using Blogic.Crm.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using static Blogic.Crm.Domain.Data.Entities.User;
using static Blogic.Crm.Infrastructure.TypeExtensions.DateTimeExtensions;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

public sealed class CreateConsultantCommandValidator : AbstractValidator<CreateConsultantCommand>
{
	private readonly DataContext _dataContext;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;

	public CreateConsultantCommandValidator(DataContext dataContext, IEmailLookupNormalizer emailLookupNormalizer,
	                                        IPhoneLookupNormalizer phoneLookupNormalizer)
	{
		_dataContext = dataContext;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;

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

			RuleFor(c => c.Email)
				.Must(EmailNotTaken)
				.WithMessage("Consultant's email address already taken.");
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

			RuleFor(c => c.Phone)
				.Must(PhoneNotTaken)
				.WithMessage("Consultant's phone number already taken.");
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
				.Must(IsBirthNumber)
				.WithMessage("Consultant's birth number format is invalid.");

			RuleFor(c => c.BirthNumber)
				.Must(BirthNumberNotTaken)
				.WithMessage("Consultant's birth number already taken.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.BirthNumber)
				.NotEmpty()
				.WithMessage("Consultant's birth number is required.");
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
			.WithMessage($"Consultant must be at least {AgeMinimumValue} years old.");
	}

	public bool EmailNotTaken(string email)
	{
		var normalizedEmail = _emailLookupNormalizer.Normalize(email)!;
		return !_dataContext.Consultants.AsNoTracking().Any(c => c.NormalizedEmail == normalizedEmail);
	}

	public bool PhoneNotTaken(string phone)
	{
		var normalizedPhone = _phoneLookupNormalizer.Normalize(phone)!;
		return !_dataContext.Consultants.AsNoTracking().Any(c => c.Phone == normalizedPhone);
	}

	public bool BirthNumberNotTaken(string birthNumber)
	{
		return !_dataContext.Consultants.AsNoTracking().Any(c => c.BirthNumber == birthNumber);
	}
}