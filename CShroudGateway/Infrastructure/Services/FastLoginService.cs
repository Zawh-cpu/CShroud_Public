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
        uint[] variants = new uint[3];
        uint a;
        for (int i = 0; i < 3; i++)
        {
            do
            {
                a = (uint)RandomNumberGenerator.GetInt32(10, 999);
                variants[i] = a;
            } while (variants.Count(x => x == a) > 1);
        }
        
        return new FastLogin()
        {
            Ipv4Address = ipAddress,
            DeviceInfo = userAgent,
            Variants = variants,
            Secret = GenerateAsciiToken(256)
        };
    }
}