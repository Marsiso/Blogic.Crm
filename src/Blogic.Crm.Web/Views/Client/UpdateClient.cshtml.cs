#nullable disable

namespace Blogic.Crm.Web.Views.Client;

public sealed class ClientUpdateViewModel
{
	public ClientUpdateViewModel()
	{
	}
	
	public ClientUpdateViewModel(long id, string originAction, ClientInput client)
	{
		Id = id;
		Client = client;
		OriginAction = originAction;
	}
	
	public ClientUpdateViewModel(long id, string originAction, ClientInput client,
	                             ValidationException validationException)
	{
		Id = id;
		Client = client;
		ValidationException = validationException;
		OriginAction = originAction;
	}

	public long Id { get; set; }
	public string OriginAction { get; set; }
	public ClientInput Client { get; set; }
	public ValidationException ValidationException { get; set; }
}