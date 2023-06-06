namespace Blogic.Crm.Web.Views.Consultant;

public sealed class GetConsultantViewModel
{
	public GetConsultantViewModel()
	{
	}
	
	public GetConsultantViewModel(ConsultantRepresentation? consultant,
	                              IEnumerable<ContractRepresentation>? managedContracts,
	                              IEnumerable<ContractRepresentation>? consultedContracts)
	{
		Consultant = consultant;
		ManagedContracts = managedContracts;
		ConsultedContracts = consultedContracts;
	}

	public ConsultantRepresentation? Consultant { get; set; }
	public IEnumerable<ContractRepresentation>? ManagedContracts { get; set; }
	public IEnumerable<ContractRepresentation>? ConsultedContracts { get; set; }
}