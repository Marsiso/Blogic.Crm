namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     Security stamp provider.
/// </summary>
public sealed class SecurityStampProvider : ISecurityStampProvider
{
    /// <summary>
    ///     The format of the generated stamp, also known as Hyphens , which consists of 36 characters.
    /// </summary>
    private const string Format = "D";

    public string GenerateSecurityStamp()
    {
        return Guid.NewGuid().ToString(Format);
    }
}