using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Commands;

public sealed record UpdateContractCommand(long Id, string RegistrationNumber, string Institution, DateTime DateConcluded,
                                           DateTime DateExpired, DateTime DateValid, long ClientId, long ManagerId ,
                                           long[] Consultants) : ICommand<Unit>;

public sealed class UpdateContractCommandHandler : ICommandHandler<UpdateContractCommand, Unit>
{
	public UpdateContractCommandHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<Unit> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
	{
		Contract? originalEntity = await _dataContext.Contracts
		                                             .AsTracking()
		                                             .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
		if (originalEntity is null)
		{
			return Unit.Value;
		}

		request.Adapt(originalEntity);
		
		_dataContext.Contracts.Update(originalEntity);
		await _dataContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}