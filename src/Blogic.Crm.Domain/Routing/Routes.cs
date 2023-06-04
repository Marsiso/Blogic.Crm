namespace Blogic.Crm.Domain.Routing;

public sealed class Routes
{
	public const string Root = "pages";
	public const string Version = "v1";
	public const string Base = $"{Root}/{Version}";

	public static class Client
	{
		public const string ClientBase = Base + "/client";
		public const string DeleteClientPrompt = ClientBase + "/delete/{id:long}/prompt";
		public const string DeleteClient = ClientBase + "/delete/{id:long}";
		public const string UpdateClient = ClientBase + "/put/{id:long}";
		public const string CreateClient = ClientBase + "/post";
		public const string GetClients = ClientBase + "/getAll";
		public const string GetClient = ClientBase + "/get/{id:long}";
	}
	
	public static class Account
	{
		public const string AccountBase = Base + "/account";
		public const string PostAccount = AccountBase + "/post";
		public const string PostClient = AccountBase + "/post/client";
	}
}