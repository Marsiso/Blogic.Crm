namespace Blogic.Crm.Infrastructure.Commands;

public sealed record CreateContractCommand(string RegistrationNumber, string Institution, DateTime DateConcluded,
                                           DateTime DateExpired, DateTime DateValid, long ClientId, long ManagerId) : ICommand<Entity>;

public sealed class CreateContractCommandHandler : ICommandHandler<CreateContractCommand, Entity>
{
	public CreateContractCommandHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public Task<Entity> Handle(CreateContractCommand request, CancellationToken cancellationToken)
	{
		Contract contractEntity = request.Adapt<Contract>();

		_dataContext.Contracts.Add(contractEntity);
		_dataContext.SaveChanges();

		ContractConsultant contractConsultantEntity = new ContractConsultant
		{
			ContractId = contractEntity.Id,
			ConsultantId = request.ManagerId
		};
		
		_dataContext.ContractConsultants.Add(contractConsultantEntity);
		_dataContext.SaveChanges();

		return Task.FromResult((Entity)contractEntity);
	}
}