namespace Blogic.Crm.Infrastructure.Validators;

/// <summary>
///     Validations for the <see cref="UpdateContractCommand" /> command.
/// </summary>
public class UpdateContractQueryValidator : AbstractValidator<UpdateContractCommand>
{
    public UpdateContractQueryValidator(DataContext dataContext)
    {
        #region RegistrationNumber

        When(c => IsNotNullOrEmpty(c.RegistrationNumber), () =>
        {
            RuleFor(c => c.RegistrationNumber)
                .Must(IsValidRegistrationNumber)
                .WithMessage("Registration number has invalid format.");

            RuleFor(c => c.RegistrationNumber)
                .Must((c, registrationNumber) => RegistrationNumberNotTaken(c.Entity, registrationNumber, dataContext))
                .WithMessage("Contract with the given registration number already exists.");
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

        #region ClientIdentifier

        When(c => c.ClientId > 0, () =>
        {
            RuleFor(c => c.ClientId)
                .Must(id => ClientExists(id, dataContext))
                .WithMessage(c => $"Client with ID {c.ClientId} doesn't exist.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.ClientId)
                .GreaterThan(0)
                .WithMessage("Client identifier is required.");
        });

        #endregion


        #region ManagerIdentifier

        When(c => c.ManagerId > 0, () =>
        {
            RuleFor(c => c.ManagerId)
                .Must(id => ConsultantExists(new Entity(id), dataContext))
                .WithMessage(c => $"Manager with ID {c.ManagerId} doesn't exist.");
        }).Otherwise(() =>
        {
            RuleFor(c => c.ManagerId)
                .GreaterThan(0)
                .WithMessage("Manager ID is required.");
        });

        #endregion

        #region DateConcluded

        RuleFor(c => c.DateConcluded)
            .GreaterThanOrEqualTo(MinimalDateConcluded)
            .WithMessage($"Date of conclusion must be the same or later than {MinimalDateConcluded:dd/MM/yyyy}.");

        RuleFor(c => c.DateConcluded)
            .LessThanOrEqualTo(c => c.DateValid)
            .WithMessage("Date of conclusion must be the same or earlier than the validity date.");

        #endregion

        #region DateValid

        RuleFor(c => c.DateValid)
            .GreaterThanOrEqualTo(MinimalDateConcluded)
            .WithMessage($"Date of validity must be the same or later than {MinimalDateConcluded:dd/MM/yyyy}.");

        RuleFor(c => c.DateValid)
            .LessThanOrEqualTo(c => c.DateExpired)
            .WithMessage("Date of validity must be the same or earlier than the expiry date.");

        #endregion

        #region DateExpired

        RuleFor(c => c.DateExpired)
            .GreaterThanOrEqualTo(MinimalDateConcluded)
            .WithMessage($"Date of expiration must be the same or later than {MinimalDateConcluded:dd/MM/yyyy}.");

        #endregion

        #region ConsultantIdentifiers

        When(c => IsNotNullOrEmpty(c.ConsultantIds), () =>
        {
            RuleFor(c => c.ConsultantIds)
                .Matches(@"^((\s*\d+\s*,)*(\s*\d+\s*,?\s*))?$")
                .WithMessage("Consultant IDs have incorrect format.")
                .Must(idsAsString => ConsultantsExist(idsAsString, dataContext))
                .WithMessage("Consultant ID/IDs doesn't exist.");
        });

        #endregion
    }
}