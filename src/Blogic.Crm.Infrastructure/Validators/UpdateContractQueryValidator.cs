using Blogic.Crm.Infrastructure.Commands;
using Microsoft.EntityFrameworkCore;
using static Blogic.Crm.Domain.Data.Entities.Contract;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Validators;

public class UpdateContractQueryValidator : AbstractValidator<UpdateContractCommand>
{
	private readonly DataContext _dataContext;
	
	public UpdateContractQueryValidator(DataContext dataContext)
	{
				_dataContext = dataContext;
		When(c => IsNotNullOrEmpty(c.RegistrationNumber), () =>
		{
			RuleFor(c => c.RegistrationNumber)
				.Must(HasValidFormat)
				.WithMessage("The registration number has invalid format.");
			
			RuleFor(c => c.RegistrationNumber)
				.Must((c, rn) => RegistrationNumberNotTaken(c.Id, rn))
				.WithMessage("Contract with the given registration number already exists.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.RegistrationNumber)
				.NotEmpty()
				.WithMessage("The registration number is required.");
		});
		
		When(c => IsNotNullOrEmpty(c.Institution), () =>
		{
		}).Otherwise(() =>
		{
			RuleFor(c => c.Institution)
				.NotEmpty()
				.WithMessage("Institution is required.");
		});

		When(c => c.ClientId > 0, () =>
		{
			RuleFor(c => c.ClientId)
				.Must(ClientExists)
				.WithMessage(c => $"Client with ID {c.ClientId} doesn't exist.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.ClientId)
				.GreaterThan(0)
				.WithMessage("A client identifier is required.");
		});
		
		When(c => c.ManagerId > 0, () =>
		{
			RuleFor(c => c.ManagerId)
				.Must(ConsultantExists)
				.WithMessage(c => $"Manager with ID {c.ManagerId} doesn't exist.");
		}).Otherwise(() =>
		{
			RuleFor(c => c.ManagerId)
				.GreaterThan(0)
				.WithMessage("A manager identifier is required.");

		});
		
		RuleFor(c => c.DateConcluded)
			.GreaterThanOrEqualTo(MinimalDateConcluded)
			.WithMessage($"The date of conclusion must be the same or later than {MinimalDateConcluded:dd/MM/yyyy}.");
		
		RuleFor(c => c.DateValid)
			.GreaterThanOrEqualTo(MinimalDateConcluded)
			.WithMessage($"The date of validity must be the same or later than {MinimalDateConcluded:dd/MM/yyyy}.");
		
		RuleFor(c => c.DateExpired)
			.GreaterThanOrEqualTo(MinimalDateConcluded)
			.WithMessage($"The date of expiration must be the same or later than {MinimalDateConcluded:dd/MM/yyyy}.");

		RuleFor(c => c.DateConcluded)
			.LessThanOrEqualTo(c => c.DateValid)
			.WithMessage("The date of conclusion must be the same or earlier than the validity date.");

		RuleFor(c => c.DateValid)
			.LessThanOrEqualTo(c => c.DateExpired)
			.WithMessage("The date of validity must be the same or earlier than the expiry date.");
	}
	
	private static bool HasValidFormat(string registrationNumber)
	{
		return Guid.TryParse(registrationNumber, out _);
	}
	
	private bool RegistrationNumberNotTaken(long id, string registrationNumber)
	{
		return !_dataContext.Contracts
		                    .AsNoTracking()
		                    .Any(c => c.Id != id && c.RegistrationNumber == registrationNumber);
	}

	private bool ClientExists(long id)
	{
		return _dataContext.Clients
		                   .AsNoTracking()
		                   .Any(c => c.Id == id);
	}
	
	private bool ConsultantExists(long id)
	{
		return _dataContext.Consultants
		                   .AsNoTracking()
		                   .Any(c => c.Id == id);
	}
}