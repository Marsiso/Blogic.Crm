#nullable disable

namespace Blogic.Crm.Web.Views.Client;

public sealed class ClientCreateViewModel
{
	public ClientCreateViewModel() { }

	public ClientCreateViewModel(ClientInput client, ValidationException validationException)
	{
		Client = client;
		ValidationException = validationException;
	}

	public ClientInput Client { get; set; }
	public ValidationException ValidationException { get; set; }
}