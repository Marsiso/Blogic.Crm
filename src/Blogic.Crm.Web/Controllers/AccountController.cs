using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Domain.Exceptions;
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
	public IActionResult Register()
	{
		return View(new RegisterViewModel());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult>Register(RegisterViewModel viewModel, CancellationToken cancellationToken)
	{
		try
		{
			CreateClientCommand createClientCommand = viewModel.Client.Adapt<CreateClientCommand>();
			Entity entity = await _sender.Send(createClientCommand, cancellationToken);
			return RedirectToAction("Detail", "Client", new { entity.Id });
		}
		catch (ValidationException exception)
		{
			return View("Register", new RegisterViewModel(viewModel.Client, exception));
		}
		catch (Exception)
		{
			return View(new RegisterViewModel());
		}
	}
}