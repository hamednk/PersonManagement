using ApiClient.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Protos;

namespace ApiClient.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(PersonGrpcClientService clientService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PersonResponse>> CreatePerson([FromBody] CreatePersonRequest request)
    {
        try
        {
            var response = await clientService.CreatePersonAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode(500, ex.Status.Detail);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonResponse>> GetPerson(string id)
    {
        try
        {
            var response = await clientService.GetPersonAsync(id);
            return Ok(response);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonResponse>>> GetAllPersons([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var persons = await clientService.GetAllPersonsAsync(page, pageSize);
        return Ok(persons);
    }

    [HttpPut]
    public async Task<ActionResult<PersonResponse>> UpdatePerson([FromBody] UpdatePersonRequest request)
    {
        try
        {
            var response = await clientService.UpdatePersonAsync(request);
            return Ok(response);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeletePerson(string id)
    {
        try
        {
            var success = await clientService.DeletePersonAsync(id);
            return Ok(success);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return NotFound();
        }
    }
}