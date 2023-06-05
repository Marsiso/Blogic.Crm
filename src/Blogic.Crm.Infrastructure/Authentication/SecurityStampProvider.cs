namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     Security stamp provider.
/// </summary>
public sealed class SecurityStampProvider : ISecurityStampProvider
{
	public const string Format = "D";

	public string GenerateSecurityStamp()
	{
		return Guid.NewGuid().ToString(Format);
	}
}