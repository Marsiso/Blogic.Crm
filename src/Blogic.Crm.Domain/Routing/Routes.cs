namespace Blogic.Crm.Domain.Routing;

public sealed class Routes
{
	public const string Root = "pages";
	public const string Version = "v1";
	public const string Base = $"{Root}/{Version}";

	public static class Client
	{
		public const string ClientBase = Base + "/client";
		public const string DeleteClientPrompt = ClientBase + "/{id:long}/account/delete/prompt";
		public const string DeleteClient = ClientBase + "/{id:long}/account/delete";
		public const string UpdateClient = ClientBase + "/{id:long}/account/update";
		public const string CreateClient = ClientBase + "/account/create";
		public const string GetClients = ClientBase + "/dashboard";
		public const string GetClient = ClientBase + "/{id:long}/account";
		public const string ExportClients = ClientBase + "/dashboard/export";
	}
	
	public static class Account
	{
		public const string AccountBase = Base + "/account";
		public const string PostAccount = AccountBase + "/post";
		public const string PostClient = AccountBase + "/post/client";
	}
}