using Grpc.Net.Client;

namespace CShroudDAW.Core.Interfaces;

public interface IGrpcPool
{
    GrpcChannel Get(string address);
}