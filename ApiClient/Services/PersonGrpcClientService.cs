using Grpc.Net.Client;

using Protos;

namespace ApiClient.Services;

public class PersonGrpcClientService
{
    private readonly PersonServiceProto.PersonServiceProtoClient _client;
    private readonly IConfiguration _config;

    public PersonGrpcClientService(IConfiguration config)
    {
        _config = config;
        var channel = GrpcChannel.ForAddress(_config["GrpcServer:Address"]!);
        _client = new PersonServiceProto.PersonServiceProtoClient(channel);
    }

    public async Task<PersonResponse> CreatePersonAsync(CreatePersonRequest request)
    {
        return await _client.CreatePersonAsync(request);
    }

    public async Task<PersonResponse> GetPersonAsync(string id)
    {
        return await _client.GetPersonAsync(new GetPersonRequest { Id = id });
    }

    public async Task<IEnumerable<PersonResponse>> GetAllPersonsAsync(int pageIndex = 1, int pageSize = 10)
    {
        var response = await _client.GetAllPersonsAsync(new GetAllPersonsRequest
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        });
        return response.Persons;
    }

    public async Task<PersonResponse> UpdatePersonAsync(UpdatePersonRequest request)
    {
        return await _client.UpdatePersonAsync(request);
    }

    public async Task<bool> DeletePersonAsync(string id)
    {
        var response = await _client.DeletePersonAsync(new DeletePersonRequest { Id = id });
        return response.Success;
    }

}