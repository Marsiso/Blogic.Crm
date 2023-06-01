using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Domain.Exceptions;

namespace Blogic.Crm.Domain.Data.Dtos;

public sealed record UpdateClientResult(Entity? Entity);

public static class UpdateClientResultExtensions
{
	public static bool IsSuccess(this UpdateClientResult updateClientResult)
	{
		return updateClientResult is { Entity.Id: > 0 };
	}
	
	public static bool IsFailure(this UpdateClientResult updateClientResult)
	{
		return !updateClientResult.IsSuccess();
	}
}