using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Domain.Exceptions;
using Blogic.Crm.Domain.Routing;
using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Web.Views.Account;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Web.Controllers;

public sealed class AccountController : Controller
{
	public AccountController(ISender sender)
	{
		_sender = sender;
	}

	private readonly ISender _sender;

	[HttpGet]
	[Route(Routes.Account.PostAccount)]
	public IActionResult PostAccount()
	{
		return View(new AccountRegisterIndexViewModel());
	}

	[HttpPost]
	[Route(Routes.Account.PostClient)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult>PostClient(AccountRegisterIndexViewModel indexViewModel, CancellationToken cancellationToken)
	{
		try
		{
			CreateClientCommand createClientCommand = indexViewModel.Client.Adapt<CreateClientCommand>();
			Entity entity = await _sender.Send(createClientCommand, cancellationToken);
			return RedirectToAction("GetClient", "Client", new { entity.Id });
		}
		catch (ValidationException exception)
		{
			return View(nameof(PostAccount), new AccountRegisterIndexViewModel(indexViewModel.Client, exception));
		}
		catch (Exception)
		{
			return View(nameof(PostAccount), new AccountRegisterIndexViewModel());
		}
	}
}