namespace Blogic.Crm.Web.Views.Client;

public sealed class ClientUpdateViewModel
{
	public ClientUpdateViewModel(long id, ClientInput client)
	{
		Id = id;
		Client = client;
	}

	public ClientUpdateViewModel(long id, ClientInput client, ValidationException? validationException)
	{
		Id = id;
		Client = client;
		ValidationException = validationException;
	}

	public long Id { get; set; }
	public ClientInput Client { get; set; }
	public ValidationException? ValidationException { get; set; }
}