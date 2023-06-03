#nullable disable

using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Exceptions;

namespace Blogic.Crm.Web.Views.Account;

public sealed class AccountRegisterIndexViewModel
{
	public AccountRegisterIndexViewModel()
	{
		Client = new ClientInput();
	}
	
	public AccountRegisterIndexViewModel(ClientInput client, ValidationException validationException)
	{
		Client = client;
		ValidationException = validationException;
	}

	public ClientInput Client { get; set; }
	public ValidationException ValidationException { get; set; }
}