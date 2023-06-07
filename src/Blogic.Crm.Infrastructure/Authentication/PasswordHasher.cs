using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     A password hashing provider that uses PBKDF2 key derivation and a randomly generated salt.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    /// <summary>
    ///     Key size in bytes.
    /// </summary>
    private const int KeySize = 32;

    /// <summary>
    ///     Salt size in bytes.
    /// </summary>
    private const int SaltSize = 16;

    /// <summary>
    ///     Number of cycles performed by the algorithm during key derivation.
    /// </summary>
    private const int Cycles = 1_572_864;

    /// <summary>
    ///     A separator used to store a key including the salt used in its derivation for storage purposes.
    /// </summary>
    private const char Separator = ';';

    /// <summary>
    ///     Type of algorithm used for key derivation.
    /// </summary>
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string HashPassword(ReadOnlySpan<char> password)
    {
        Debug.Assert(!password.IsEmpty);
        Debug.Assert(!password.IsWhiteSpace());

        // Encode password. 
        Span<byte> passwordBytes = stackalloc byte[password.Length];
        Encoding.UTF8.GetBytes(password, passwordBytes);

        // Generate random salt.
        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.Fill(salt);

        // Derive key.
        Span<byte> key = stackalloc byte[KeySize];
        Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, key, Cycles, Algorithm);

        // Return the key including the salt used to derive the key.
        return $"{Convert.ToBase64String(key)};{Convert.ToBase64String(salt)}";
    }

    public PasswordVerificationResult VerifyPassword(ReadOnlySpan<char> password, ReadOnlySpan<char> passwordHash)
    {
        Debug.Assert(!password.IsEmpty);
        Debug.Assert(!password.IsWhiteSpace());
        Debug.Assert(!passwordHash.IsEmpty);
        Debug.Assert(!passwordHash.IsWhiteSpace());

        // Encode password. 
        Span<byte> passwordBytes = stackalloc byte[password.Length];
        Encoding.UTF8.GetBytes(password, passwordBytes);

        // Separate the key and salt using the separator.
        var delimiter = passwordHash.IndexOf(Separator);
        if (delimiter == -1) throw new FormatException("Hash does not contain a key and salt separator.");

        var passwordHashKey = Convert.FromBase64String(passwordHash[..delimiter++].ToString());
        var passwordHashSalt = Convert.FromBase64String(passwordHash[delimiter..].ToString());

        // Derive key. 
        Span<byte> key = stackalloc byte[KeySize];
        Rfc2898DeriveBytes.Pbkdf2(passwordBytes, passwordHashSalt, key, Cycles, Algorithm);

        // Compare key derivation and encrypted password at fixed time intervals
        return CryptographicOperations.FixedTimeEquals(key, passwordHashKey)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Fail;
    }
}