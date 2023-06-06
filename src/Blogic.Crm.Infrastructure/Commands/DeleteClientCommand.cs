using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Deletes the persisted client.
/// </summary>
/// <param name="Id">Provided unique identifier to distinct between clients.</param>
public sealed record DeleteClientCommand(Entity Entity) : ICommand<Unit>;

/// <summary>
///     Handles the <see cref="DeleteClientCommandHandler" /> command.
/// </summary>
public sealed class DeleteClientCommandHandler : ICommandHandler<DeleteClientCommand, Unit>
{
	private readonly DataContext _dataContext;

	public DeleteClientCommandHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
	{
		// Retrieve the persisted client using the provided ID.
		var clientEntity = await _dataContext.Clients
		                                     .AsTracking()
		                                     .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

		// When the client isn't persisted then take no action and return else delete the persisted client.
		if (clientEntity != null)
		{
			_dataContext.Clients.Remove(clientEntity);
			await _dataContext.SaveChangesAsync(cancellationToken);
		}

		return Unit.Value;
	}
}