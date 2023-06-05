namespace Blogic.Crm.Domain.Routing;

public sealed class Routes
{
	public const string Root = "pages";
	public const string Version = "v1";
	public const string Base = $"{Root}/{Version}";

	public static class Client
	{
		public const string ClientBase = Base + "/client";
		public const string DeletePrompt = ClientBase + "/{id:long}/account/delete/prompt";
		public const string Delete = ClientBase + "/{id:long}/account/delete";
		public const string Update = ClientBase + "/{id:long}/account/update";
		public const string Create = ClientBase + "/account/create";
		public const string GetAll = ClientBase + "/dashboard";
		public const string Get = ClientBase + "/{id:long}/account";
		public const string Export = ClientBase + "/dashboard/export";
	}

	public static class Consultant
	{
		public const string ConsultantBase = Base + "/consultant";
		public const string DeletePrompt = ConsultantBase + "/{id:long}/account/delete/prompt";
		public const string Delete = ConsultantBase + "/{id:long}/account/delete";
		public const string Update = ConsultantBase + "/{id:long}/account/update";
		public const string Create = ConsultantBase + "/account/create";
		public const string GetAll = ConsultantBase + "/dashboard";
		public const string Get = ConsultantBase + "/{id:long}/account";
		public const string Export = ConsultantBase + "/dashboard/export";
	}
	
	public static class Contract
	{
		public const string ContractBase = Base + "/contract";
		public const string DeletePrompt = ContractBase + "/{id:long}/account/delete/prompt";
		public const string Delete = ContractBase + "/{id:long}/account/delete";
		public const string Update = ContractBase + "/{id:long}/account/update";
		public const string Create = ContractBase + "/account/create";
		public const string GetAll = ContractBase + "/dashboard";
		public const string Get = ContractBase + "/{id:long}/account";
		public const string Export = ContractBase + "/dashboard/export";
	}
}