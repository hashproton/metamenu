using Application.UseCases.TagGroups.Commands;
using Application.UseCases.TagGroups.Queries;

namespace Api.Controllers;

public class TagGroupsController(ISender mediator) : BaseController
{
    [HttpPost]
    public async Task<ActionResult> CreateTagGroup([FromBody] CreateTagGroupCommand command)
    {
        var result = await mediator.Send(command);

        return CreatedAtAction(nameof(GetTagGroupById), new { id = result }, result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<GetAllTagGroupsQueryResponse>>> GetAllTagGroups(
        [FromQuery] int tenantId,
        [FromQuery] PaginatedQuery paginatedQuery)
    {
        var result = await mediator.Send(new GetAllTagGroupsQuery(tenantId, paginatedQuery));

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetTagGroupByIdQueryResponse>> GetTagGroupById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetTagGroupByIdQuery(id));

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTagGroup(
        [FromRoute] int id,
        [FromBody] UpdateTagGroupCommand command)
    {
        command.Id = id;

        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTagGroup([FromRoute] int id)
    {
        await mediator.Send(new DeleteTagGroupCommand(id));

        return NoContent();
    }
}