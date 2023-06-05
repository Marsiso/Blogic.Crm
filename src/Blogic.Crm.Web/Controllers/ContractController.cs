#nullable disable

using Blogic.Crm.Infrastructure.Persistence;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Web.Views.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Blogic.Crm.Web.Controllers;

public sealed class ContractController : Controller
{
	private readonly ISender _sender;

	public ContractController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet(Routes.Contract.GetAll)]
	[HttpPost(Routes.Contract.GetAll)]
	public async Task<IActionResult> GetContracts(ContractQueryString queryParameters,
	                                              CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve paginated contract representation for the view model.
			GetContractRepresentationsQuery query = new(queryParameters);
			PaginatedList<ContractRepresentation> paginatedContracts = await _sender.Send(query, cancellationToken);
		
			// Build the view model and return it to the client.
			return View(new GetContractsViewModel { Contracts  = paginatedContracts, QueryString = queryParameters });
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(GetContracts));

			return RedirectToAction(nameof(Index), "Home", new { });
		}
	}

	public IActionResult GetContract()
	{
		throw new NotImplementedException();
	}

	public IActionResult CreateContract()
	{
		throw new NotImplementedException();
	}

	public IActionResult UpdateContract()
	{
		throw new NotImplementedException();
	}

	public IActionResult DeleteContractPrompt()
	{
		throw new NotImplementedException();
	}

	public IActionResult ExportContracts()
	{
		throw new NotImplementedException();
	}
}