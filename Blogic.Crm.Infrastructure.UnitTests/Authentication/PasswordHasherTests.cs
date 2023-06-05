using Blogic.Crm.Infrastructure.Authentication;
using FluentAssertions;
using Xunit;

namespace Blogic.Crm.Infrastructure.UnitTests.Authentication;

public sealed class PasswordHasherTests
{
	[Fact]
	public void HashPassword_WhenNullReferenceAsPassword_ThenThrowException()
	{
		// Arrange
		PasswordHasher passwordHasher = new();

		void Action()
		{
			passwordHasher.HashPassword(null);
		}

		// Act
		var exception = Record.Exception(Action);

		// Assert
		exception.Should().NotBeNull();
	}

	[Fact]
	public void HashPassword_WhenEmptyStringAsPassword_ThenThrowException()
	{
		// Arrange
		PasswordHasher passwordHasher = new();

		void Action()
		{
			passwordHasher.HashPassword(string.Empty);
		}

		// Act
		var exception = Record.Exception(Action);

		// Assert
		exception.Should().NotBeNull();
	}

	[Fact]
	public void HashPassword_WhenValidPassword_ThenReturnHash()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string password = "Pass123$";

		// Act
		var passwordHash = passwordHasher.HashPassword(password);

		// Assert
		passwordHash.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void HashPassword_WhenValidPassword_ThenReturnHash_ThatContainsDelimiter()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string password = "Pass123$";
		const string delimiter = ";";

		// Act
		var passwordHash = passwordHasher.HashPassword(password);

		// Assert
		passwordHash.Should().NotBeNullOrEmpty();
		passwordHash.Should().Contain(delimiter);
	}

	[Fact]
	public void HashPassword_WhenValidPassword_ThenReturnHash_ThatContainsKeyAndSaltSeparatedByDelimiter()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string password = "Pass123$";
		const string delimiter = ";";

		// Act
		var passwordHash = passwordHasher.HashPassword(password);
		var delimiterIndex = passwordHash.IndexOf(delimiter, StringComparison.Ordinal);
		var passwordHashKey = passwordHash[..delimiterIndex++];
		var passwordHashSalt = passwordHash[delimiterIndex..];

		// Assert
		passwordHash.Should().NotBeNullOrEmpty();
		delimiterIndex.Should().NotBe(-1);
		passwordHashKey.Should().NotBeNullOrEmpty();
		passwordHashSalt.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void VerifyPassword_WhenNullReferenceAsPassword_ThenThrowException()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string passwordHash = "KEY;SALT";

		void Action()
		{
			passwordHasher.VerifyPassword(null, passwordHash);
		}

		// Act
		var exception = Record.Exception(Action);

		// Assert
		exception.Should().NotBeNull();
	}

	[Fact]
	public void VerifyPassword_WhenEmptyStringAsPassword_ThenThrowException()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string passwordHash = "KEY;SALT";

		void Action()
		{
			passwordHasher.VerifyPassword(string.Empty, passwordHash);
		}

		// Act
		var exception = Record.Exception(Action);

		// Assert
		exception.Should().NotBeNull();
	}

	[Fact]
	public void VerifyPassword_WhenNullReferenceAsPasswordHash_ThenThrowException()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string password = "Pass123$";

		void Action()
		{
			passwordHasher.VerifyPassword(password, null);
		}

		// Act
		var exception = Record.Exception(Action);

		// Assert
		exception.Should().NotBeNull();
	}

	[Fact]
	public void VerifyPassword_WhenEmptyStringAsPasswordHash_ThenThrowException()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string password = "Pass123$";

		void Action()
		{
			passwordHasher.VerifyPassword(password, string.Empty);
		}

		// Act
		var exception = Record.Exception(Action);

		// Assert
		exception.Should().NotBeNull();
	}

	[Fact]
	public void AssignableTo_IPasswordHasher_Abstraction()
	{
		// Arrange
		PasswordHasher passwordHasher = new();

		// Assert
		passwordHasher.Should().BeAssignableTo<IPasswordHasher>();
	}

	[Fact]
	public void HashPassword_WhenValidPassword_ThenReturnUniquePasswordHash()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const int runs = 5;
		var passwordHashes = new string[5];
		const string password = "Pass123$";

		// Act & Assert
		for (var index = 0; index < runs; index++)
		{
			passwordHashes[index] = passwordHasher.HashPassword(password);
		}

		// Assert
		passwordHashes.Should().OnlyContain(passwordHash => !string.IsNullOrEmpty(passwordHash));
		passwordHashes.Should().OnlyHaveUniqueItems();
	}

	[Fact]
	public void VerifyPassword_WhenValidPassword_ThenReturnSuccess()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string password = "Pass123$";

		// Act
		var passwordHash = passwordHasher.HashPassword(password);
		var verificationResult = passwordHasher.VerifyPassword(password, passwordHash);

		// Assert
		verificationResult.Should().Be(PasswordVerificationResult.Success);
	}

	[Fact]
	public void HashPassword_WhenInvalidPassword_ThenReturnFail()
	{
		// Arrange
		PasswordHasher passwordHasher = new();
		const string password = "Pass123$";

		// Act
		var passwordHash = passwordHasher.HashPassword(password);
		var verificationResult = passwordHasher.VerifyPassword(string.Format("{0}{0}", password), passwordHash);

		// Assert
		passwordHash.Should().NotBeNullOrEmpty();
		verificationResult.Should().Be(PasswordVerificationResult.Fail);
	}
}