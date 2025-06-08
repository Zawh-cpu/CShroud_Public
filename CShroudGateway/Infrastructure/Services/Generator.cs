using System.Security.Cryptography;

namespace CShroudGateway.Infrastructure.Services;

public static class Generator
{
    public const string Alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomString(int length)
    {
        return RandomNumberGenerator.GetString(Alph, length);
    }
}