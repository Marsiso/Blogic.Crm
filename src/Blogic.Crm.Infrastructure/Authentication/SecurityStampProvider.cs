namespace Blogic.Crm.Infrastructure.Authentication;

public sealed class SecurityStampProvider : ISecurityStampProvider
{
	public const string Format = "D";
	
	public string GenerateSecurityStamp()
	{
		return Guid.NewGuid().ToString(Format);
	}
}