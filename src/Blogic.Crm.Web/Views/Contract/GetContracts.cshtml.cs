#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Web.Views.Contract;

public sealed class GetContractsViewModel
{
	public PaginatedList<ContractRepresentation> Contracts { get; set; }
	public ContractQueryString QueryString { get; set; }
}