using Grpc.Net.Client;

namespace CShroudGateway.Core.Interfaces;

public interface IGrpcPool
{
    GrpcChannel Get(string address);
}