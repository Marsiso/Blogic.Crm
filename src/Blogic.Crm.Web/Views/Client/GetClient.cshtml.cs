namespace Blogic.Crm.Web.Views.Client;

public sealed class GetClientViewModel
{
	public GetClientViewModel(Domain.Data.Entities.Client? client)
	{
		Client = client;
	}
	
	public Domain.Data.Entities.Client? Client { get; set; }
}