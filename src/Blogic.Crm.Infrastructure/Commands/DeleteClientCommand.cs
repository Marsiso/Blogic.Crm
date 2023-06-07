namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     The command to remove the client from the database.
/// </summary>
public sealed record DeleteClientCommand(Entity Entity) : ICommand<Unit>;

/// <summary>
///     Processes the  <see cref="DeleteClientCommandHandler" /> command.
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
        // Get the client from the database.
        var client = await _dataContext.Clients
            .AsTracking()
            .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

        // If the client is not found, do not perform any action and return,
        // otherwise remove the client from the database and save the changes.
        if (client is null) return Unit.Value;

        _dataContext.Clients.Remove(client);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}