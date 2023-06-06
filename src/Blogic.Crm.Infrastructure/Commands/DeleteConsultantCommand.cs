using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Deletes the persisted consultant.
/// </summary>
/// <param name="Id">Provided unique identifier to distinct between consultants.</param>
public sealed record DeleteConsultantCommand(Entity Entity) : ICommand<Unit>;

/// <summary>
///     Handles the <see cref="DeleteConsultantCommand" /> command.
/// </summary>
public sealed class DeleteConsultantCommandHandler : ICommandHandler<DeleteConsultantCommand, Unit>
{
	private readonly DataContext _dataContext;

	public DeleteConsultantCommandHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<Unit> Handle(DeleteConsultantCommand request, CancellationToken cancellationToken)
	{
		// Retrieve the persisted consultant using the provided ID.
		var consultantEntity = await _dataContext.Consultants
		                                         .AsTracking()
		                                         .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

		// When the consultant isn't persisted then take no action and return else delete the persisted consultant.
		if (consultantEntity != null)
		{
			_dataContext.Consultants.Remove(consultantEntity);
			await _dataContext.SaveChangesAsync(cancellationToken);
		}

		return Unit.Value;
	}
}