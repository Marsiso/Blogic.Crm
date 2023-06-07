using Blogic.Crm.Infrastructure.TypeExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

/// <summary>
///     Validations for the <see cref="Contract" /> model.
/// </summary>
public sealed class ContractValidator : AbstractValidator<Contract>
{
    public ContractValidator()
    {
        #region RegistrationNumber

        When(c => IsNotNullOrEmpty(c.RegistrationNumber), () =>
        {
            RuleFor(c => c.RegistrationNumber)
                .Must(IsValidRegistrationNumber)
                .WithMessage("Registration number has invalid format.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.RegistrationNumber)
                .NotEmpty()
                .WithMessage("Registration number is required.");
        });

        #endregion

        #region Institution

        When(c => IsNotNullOrEmpty(c.Institution), () => { }).Otherwise(() =>
        {
            RuleFor(c => c.Institution)
                .NotEmpty()
                .WithMessage("Institution is required.");
        });

        #endregion

        #region DateConcluded

        RuleFor(c => c.DateConcluded)
            .LessThanOrEqualTo(c => c.DateValid)
            .WithMessage("The date of conclusion of the contract must be the same or earlier than the validity date.");

        #endregion

        #region DateValid

        RuleFor(c => c.DateValid)
            .LessThanOrEqualTo(c => c.DateExpired)
            .WithMessage("The date of validity of the contract must be the same or earlier than the expiry date.");

        #endregion
    }
}