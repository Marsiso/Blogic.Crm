using static Blogic.Crm.Domain.Data.Entities.User;

namespace Blogic.Crm.Infrastructure.Validators;

/// <summary>
///     Validations for the <see cref="Consultant" /> model.
/// </summary>
public sealed class ConsultantValidator : AbstractValidator<Consultant>
{
    public ConsultantValidator()
    {
        #region GivenName

        When(c => IsNotNullOrEmpty(c.GivenName), () =>
        {
            RuleFor(c => c.GivenName)
                .MaximumLength(GivenNameMaximumLength)
                .WithMessage($"Given name must be at most {GivenNameMaximumLength} characters long.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.GivenName)
                .NotEmpty()
                .WithMessage("Given name is required.");
        });

        #endregion

        #region FamilyName

        When(c => IsNotNullOrEmpty(c.FamilyName), () =>
        {
            RuleFor(c => c.FamilyName)
                .MaximumLength(FamilyNameMaximumLength)
                .WithMessage($"Family name must be at most {FamilyNameMaximumLength} characters long.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.FamilyName)
                .NotEmpty()
                .WithMessage("Family name is required.");
        });

        #endregion

        #region Email

        When(c => IsNotNullOrEmpty(c.Email), () =>
        {
            RuleFor(c => c.Email)
                .MaximumLength(EmailMaximumLength)
                .WithMessage($"Email address must be at most {EmailMaximumLength} characters long.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("Email address has invalid format.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email address is required.");
        });

        #endregion

        #region Phone

        When(c => IsNotNullOrEmpty(c.Phone), () =>
        {
            RuleFor(c => c.Phone)
                .MaximumLength(PhoneMaximumLength)
                .WithMessage($"Telephone number must be at most {PhoneMaximumLength} characters long.");

            RuleFor(c => c.Phone)
                .Must(IsPhoneNumber)
                .WithMessage("Telephone number has invalid format.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.Phone)
                .NotEmpty()
                .WithMessage("Telephone number is required.");
        });

        #endregion

        #region BirthNumber

        When(c => IsNotNullOrEmpty(c.BirthNumber), () =>
        {
            RuleFor(c => c.BirthNumber)
                .MaximumLength(BirthNumberMaximumLength)
                .WithMessage($"Birth number must be at most {BirthNumberMaximumLength} characters long.");

            RuleFor(c => c.BirthNumber)
                .Matches(@"[0-9]{6}[/]?[0-9]{4}")
                .WithMessage("Birth number has invalid format.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.BirthNumber)
                .NotEmpty()
                .WithMessage("Birth number is required.");
        });

        #endregion

        #region PasswordHash

        When(c => IsNotNullOrEmpty(c.PasswordHash), () => { }).Otherwise(() =>
        {
            RuleFor(c => c.PasswordHash)
                .NotEmpty()
                .WithMessage("Password hash is required.");
        });

        #endregion

        #region DateBorn

        RuleFor(c => c.DateBorn)
            .Must(db => IsLegalAge(db, AgeMinimumValue))
            .WithMessage($"Consultant must be at least {AgeMinimumValue} years old.");

        #endregion
    }
}