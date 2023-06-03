namespace Blogic.Crm.Domain.Routing;

public sealed class Routes
{
	public const string Root = "pages";
	public const string Version = "v1";
	public const string Base = $"{Root}/{Version}";

	public static class Client
	{
		public const string ClientBase = Base + "/client";
		public const string GetClients = ClientBase;
		public const string GetClient = ClientBase + "/{id:long}";
		public const string DeleteClientPrompt = ClientBase + "/account/{id:long}/prompt";
		public const string DeleteClient= ClientBase + "/account/{id:long}";
	}
	
	public static class Account
	{
		public const string AccountBase = Base + "/account";
		public const string PostAccount = AccountBase + "/register";
		public const string PostClient = AccountBase + "/register/client";
	}
}