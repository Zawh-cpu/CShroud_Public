using CShroud.Infrastructure.Interfaces;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using CShroud.Presentation.Protos.Server;
using Google.Protobuf;
using GlobalParams = CShroud.Core.Domain.Entities.GlobalParams;

namespace CShroud.Infrastructure.Services;

public class UpdatePrimitive : IUpdatePrimitive
{
    
    public string GlobalParamsHash { get; }
    public UpdateBytes ProtoGlobalParams { get; }

    public UpdatePrimitive(GlobalParams globalParams)
    {
        var serializedGlobalParams = JsonConvert.SerializeObject(globalParams);
        GlobalParamsHash = ComputeJsonHash(serializedGlobalParams);
        ProtoGlobalParams = new UpdateBytes()
        {
            Data = ByteString.CopyFrom(Encoding.UTF8.GetBytes(serializedGlobalParams))
        };
    }
    
    string CanonicalizeJson(string json)
    {
        var parsedJson = JToken.Parse(json);
        return parsedJson.ToString(Formatting.None);
    }

    byte[] SerializeToBytes(string canonicalJson)
    {
        return Encoding.UTF8.GetBytes(canonicalJson);
    }

    string CalculateHash(byte[] data)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(data);
        return Convert.ToBase64String(hashBytes);
    }

    string ComputeJsonHash(string json)
    {
        var canonicalJson = CanonicalizeJson(json);
        var jsonBytes = SerializeToBytes(canonicalJson);
        return CalculateHash(jsonBytes);
    }
}