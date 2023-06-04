namespace Blogic.Crm.Web.Views.Client;

public sealed class DeleteClientViewModel
{
	public DeleteClientViewModel(ClientRepresentation? client, string originAction)
	{
		Client = client;
		OriginAction = originAction;
	}
	
	public ClientRepresentation? Client { get; set; }
	public string OriginAction { get; set; }
}