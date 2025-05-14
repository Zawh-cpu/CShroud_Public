using System.Security.Cryptography;
using System.Text;
using CShroud.Presentation.Protos.Server;

namespace CShroud.Infrastructure.Interfaces;

public interface IUpdatePrimitive
{
    public static string GetStringHash(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
    
    string GlobalParamsHash { get; }
    UpdateBytes ProtoGlobalParams { get; }
}