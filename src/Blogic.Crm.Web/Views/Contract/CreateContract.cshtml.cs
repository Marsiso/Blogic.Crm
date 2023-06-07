namespace Blogic.Crm.Web.Views.Contract;


public sealed class CreateContractViewModel
{
	public CreateContractViewModel()
	{
	}

	public CreateContractViewModel(ContractInput contract)
	{
		Contract = contract;
	}

	public CreateContractViewModel(ContractInput contract, ValidationException? validationException) : this()
	{
		Contract = contract;
		ValidationException = validationException;
	}

	public ContractInput Contract { get; set; } = new();
	public ValidationException? ValidationException { get; set; }
}