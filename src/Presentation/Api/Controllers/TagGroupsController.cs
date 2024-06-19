using Api.Controllers.Common;
using Application.Repositories.Common;
using Application.UseCases.Tags.Commands;
using Application.UseCases.Tags.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TagsController(ISender mediator) : BaseController
{
    [HttpPost]
    public async Task<ActionResult> CreateTag([FromBody] CreateTagCommand command)
    {
        var result = await mediator.Send(command);

        return CreatedAtAction(nameof(GetTagById), new { id = result }, result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<GetAllTagsQueryResponse>>> GetAllTags(
        [FromQuery] int tenantId, [FromQuery] PaginatedQuery paginatedQuery)
    {
        var result = await mediator.Send(new GetAllTagsQuery(tenantId, paginatedQuery));

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetTagByIdQueryResponse>> GetTagById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetTagByIdQuery(id));

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTag(
        [FromRoute] int id,
        [FromBody] UpdateTagCommand command)
    {
        command.TagId = id;

        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTag([FromRoute] int id)
    {
        await mediator.Send(new DeleteTagCommand(id));

        return NoContent();
    }
}