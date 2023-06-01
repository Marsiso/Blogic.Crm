using Blogic.Crm.Domain.Data.Entities;

namespace Blogic.Crm.Domain.Exceptions;

public sealed class EntityNotFoundException : Exception
{
	public EntityNotFoundException(string message, Entity entity)
	{
		Message = message;
		Entity = entity;
	}
	
	public EntityNotFoundException(string message, long entityId)
	{
		Message = message;
		Entity = new Entity { Id = entityId };
	}
	
	public override string Message { get; }
	public Entity Entity { get; }
}