namespace Blogic.Crm.Web.Views.Contract;

public sealed class UpdateContractViewModel
{
	public UpdateContractViewModel(long id, ContractInput contract, ValidationException? validationException)
	{
		Id = id;
		Contract = contract;
		ValidationException = validationException;
	}

	public UpdateContractViewModel(long id, ContractInput contract)
	{
		Id = id;
		Contract = contract;
	}
	public long Id { get; set; }
	public ContractInput Contract { get; set; }
	public ValidationException? ValidationException { get; set; }
}