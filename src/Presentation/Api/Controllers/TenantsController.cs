using Api.Controllers.Common;
using Application.Repositories.Common;
using Application.UseCases.Tenants.Commands;
using Application.UseCases.Tenants.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TenantsController(
    ISender mediator) : BaseController
{
    [HttpPost]
    public async Task<ActionResult> CreateTenant([FromBody] CreateTenantCommand command)
    {
        var result = await mediator.Send(command);

        return CreatedAtAction(nameof(GetTenantById), new { id = result }, result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<GetAllTenantsQueryResponse>>> GetAllTenants(
        PaginatedQuery paginatedQuery)
    {
        var result = await mediator.Send(new GetAllTenantsQuery(paginatedQuery));

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetTenantByIdQueryResponse>> GetTenantById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetTenantByIdQuery(id));

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTenant(
        [FromRoute] int id,
        [FromBody] UpdateTenantCommand command)
    {
        command.Id = id;

        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTenant([FromRoute] int id)
    {
        await mediator.Send(new DeleteTenantCommand(id));

        return NoContent();
    }
}