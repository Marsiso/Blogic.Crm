namespace Blogic.Crm.Infrastructure.Authentication;

public interface ISecurityStampProvider
{
	string GenerateSecurityStamp();
}