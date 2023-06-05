namespace Blogic.Crm.Web.Views.Contract;

public sealed class GetContractViewModel
{
	public GetContractViewModel()
	{
	}

	public GetContractViewModel(ContractRepresentation? contract, ClientRepresentation? client, ConsultantRepresentation? consultant)
	{
		Contract = contract;
		Client = client;
		Consultant = consultant;
	}

	public ContractRepresentation? Contract { get; set; }
	public ClientRepresentation? Client { get; set; }
	public ConsultantRepresentation? Consultant { get; set; }
}