using System.Security.Cryptography;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.DataProtection;

namespace CShroudGateway.Infrastructure.Services;

public class FastLoginService : IFastLoginService
{
    private static string GenerateAsciiToken(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var result = new char[length];
        byte[] buffer = new byte[length];

        RandomNumberGenerator.Fill(buffer);

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[buffer[i] % chars.Length];
        }

        return new string(result);
    }

    
    public FastLogin MakeSession(string? userAgent, string? ipAddress)
    {
        return new FastLogin()
        {
            Ipv4Address = ipAddress,
            DeviceInfo = userAgent,
            Variants = [1, 2, 3],
            Secret = GenerateAsciiToken(256)
        };
    }
}