using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Commands;

public sealed record DeleteContractCommand(Entity Entity) : ICommand<Unit>;

public sealed class DeleteContractCommandHandler : ICommandHandler<DeleteContractCommand, Unit>
{
	private readonly DataContext _dataContext;

	public DeleteContractCommandHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<Unit> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
	{
		// Retrieve the persisted contract using the provided ID.
		var contractEntity = await _dataContext.Contracts
		                                       .AsTracking()
		                                       .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

		// When the contract isn't persisted then take no action and return else delete the persisted consultant.
		if (contractEntity != null)
		{
			_dataContext.Contracts.Remove(contractEntity);
			await _dataContext.SaveChangesAsync(cancellationToken);
		}

		return Unit.Value;
	}
}