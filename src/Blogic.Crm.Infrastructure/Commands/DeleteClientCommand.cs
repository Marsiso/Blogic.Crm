using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Commands;

public sealed record DeleteClientCommand(long Id) : ICommand<Unit>;

public sealed class DeleteClientCommandHandler : ICommandHandler<DeleteClientCommand, Unit>
{
	public DeleteClientCommandHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
	{
		Client? clientEntity= await _dataContext.Clients
		                                         .AsTracking()
		                                         .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
		
		if (clientEntity != null)
		{
			_dataContext.Clients.Remove(clientEntity);
			await _dataContext.SaveChangesAsync(cancellationToken);
		}

		return Unit.Value;
	}
}