namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     Security stamp provider abstraction.
/// </summary>
public interface ISecurityStampProvider
{
	/// <summary>
	///     Generates security stamps.
	/// </summary>
	/// <returns>Security stamp.</returns>
	string GenerateSecurityStamp();
}