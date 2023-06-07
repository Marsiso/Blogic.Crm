namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     The command to remove the consultant from the database.
/// </summary>
/// <param name="Id">Provided unique identifier to distinct between consultants.</param>
public sealed record DeleteConsultantCommand(Entity Entity) : ICommand<Unit>;

/// <summary>
///     Processes the <see cref="DeleteConsultantCommand" /> command.
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
        // Get the consultant from the database.
        var consultant = await _dataContext.Consultants
            .AsTracking()
            .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

        // If the consultant is not found, do not perform any action and return,
        // otherwise remove the consultant from the database and save the changes.
        if (consultant is null) return Unit.Value;

        _dataContext.Consultants.Remove(consultant);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}