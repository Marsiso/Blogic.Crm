namespace Blogic.Crm.Web.Views.Consultant;

public sealed class GetConsultantViewModel
{
	public GetConsultantViewModel(ConsultantRepresentation? consultant,
	                              IEnumerable<ContractRepresentation>? managedContracts)
	{
		Consultant = consultant;
		ManagedContracts = managedContracts;
	}

	public ConsultantRepresentation? Consultant { get; set; }
	public IEnumerable<ContractRepresentation>? ManagedContracts { get; set; }
}