using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Google.Protobuf;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace CShroudGateway.Infrastructure.Services;

public class UpdatePrimitive : IUpdatePrimitive
{
    
    /*public string GlobalParamsHash { get; }
    public UpdateBytes ProtoGlobalParams { get; }

    public UpdatePrimitive(GlobalParams globalParams)
    {
        var serializedGlobalParams = JsonConvert.SerializeObject(globalParams);
        GlobalParamsHash = ComputeJsonHash(serializedGlobalParams);
        ProtoGlobalParams = new UpdateBytes()
        {
            Data = ByteString.CopyFrom(Encoding.UTF8.GetBytes(serializedGlobalParams))
        };
    }*/
    
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