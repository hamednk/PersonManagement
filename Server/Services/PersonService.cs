using Core.Models;
using Core.Services;

using Grpc.Core;

using Protos;

namespace Server.Services;

public class PersonService(IPersonRepository repository, ILogger<PersonService> logger) : PersonServiceProto.PersonServiceProtoBase
{
    public override async Task<PersonResponse> CreatePerson(CreatePersonRequest request, ServerCallContext context)
    {
        try
        {
            var person = new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalCode = request.NationalCode,
                BirthDate = DateTime.Parse(request.BirthDate)
            };

            var createdPerson = await repository.CreateAsync(person);

            return MapToPersonResponse(createdPerson);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating person");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<PersonResponse> GetPerson(GetPersonRequest request, ServerCallContext context)
    {
        try
        {
            var person = await repository.GetByIdAsync(request.Id);
            return person == null ? throw new RpcException(new Status(StatusCode.NotFound, "Person not found")) : MapToPersonResponse(person);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting person");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<PersonListResponse> GetAllPersons(GetAllPersonsRequest request, ServerCallContext context)
    {
        try
        {
            var persons = await repository.GetAllAsync(request.PageIndex, request.PageSize);
            var response = new PersonListResponse();

            response.Persons.AddRange(persons.Select(MapToPersonResponse));
            response.TotalCount = persons.Count();

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all persons");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<PersonResponse> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
    {
        try
        {
            var person = new Person
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalCode = request.NationalCode,
                BirthDate = DateTime.Parse(request.BirthDate)
            };

            var updatedPerson = await repository.UpdateAsync(person);

            return MapToPersonResponse(updatedPerson);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating person");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<DeletePersonResponse> DeletePerson(DeletePersonRequest request, ServerCallContext context)
    {
        try
        {
            var success = await repository.DeleteAsync(request.Id);
            return new DeletePersonResponse { Success = success };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting person");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    private static PersonResponse MapToPersonResponse(Person person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            NationalCode = person.NationalCode,
            BirthDate = person.BirthDate.ToString("yyyy-MM-dd")
        };
    }
}