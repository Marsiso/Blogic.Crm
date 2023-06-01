using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Domain.Exceptions;

namespace Blogic.Crm.Domain.Data.Dtos;

public sealed record CreateClientResult(Entity? Entity);

public static class RegisterClientResultExtensions
{
	public static bool IsSuccess(this CreateClientResult createClientResult)
	{
		return createClientResult is { Entity.Id: > 0 };
	}
	
	public static bool IsFailure(this CreateClientResult createClientResult)
	{
		return !createClientResult.IsSuccess();
	}
}