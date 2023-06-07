namespace Blogic.Crm.Web.Views.Contract;

public sealed class GetContractViewModel
{
	public GetContractViewModel()
	{
	}

	public GetContractViewModel(ContractRepresentation? contract, ClientRepresentation? client, ConsultantRepresentation? manager, IEnumerable<ConsultantRepresentation>? consultants)
	{
		Contract = contract;
		Client = client;
		Manager = manager;
		Consultants = consultants;
	}

	public ContractRepresentation? Contract { get; set; }
	public ClientRepresentation? Client { get; set; }
	public ConsultantRepresentation? Manager { get; set; }
	public IEnumerable<ConsultantRepresentation>? Consultants { get; set; }
}