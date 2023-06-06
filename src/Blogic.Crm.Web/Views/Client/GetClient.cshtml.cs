namespace Blogic.Crm.Web.Views.Client;

public sealed class GetClientViewModel
{
	public GetClientViewModel(ClientRepresentation? client, List<ContractRepresentation>? ownedContracts)
	{
		Client = client;
		OwnedContracts = ownedContracts;
	}

	public ClientRepresentation? Client { get; set; }
	public List<ContractRepresentation>? OwnedContracts { get; set; }
}