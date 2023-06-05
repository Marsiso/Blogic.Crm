using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Web.Views.Contract;


public sealed class CreateContractViewModel
{
	public ContractInput Contract { get; set; } = new();
	public ValidationException? ValidationException { get; set; }
}