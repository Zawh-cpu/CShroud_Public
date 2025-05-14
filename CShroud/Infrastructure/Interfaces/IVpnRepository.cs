using CShroud.Infrastructure.Data.Entities;
using Google.Protobuf;
using Xray.App.Stats.Command;
using Xray.Common.Serial;

namespace CShroud.Infrastructure.Interfaces;

public interface IVpnRepository
{
    public static TypedMessage ToTypedMessage(IMessage message)
    {
        ByteString serializedMessage = message.ToByteString();
    
        return new TypedMessage
        {
            Type = message.Descriptor.FullName,
            Value = serializedMessage
        };
    }

    Task<bool> AddKey(uint vpnLevel, string uuid, string protocolId);

    Task<bool> DelKey(string uuid, string protocolId);
    
    Task<SysStatsResponse?> GetSysStat();
}