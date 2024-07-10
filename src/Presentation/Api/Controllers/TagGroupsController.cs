using Application.UseCases.TagGroups.Commands;
using Application.UseCases.TagGroups.Queries;
using Application.UseCases.TagGroups.Queries.Common;

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
    public async Task<ActionResult<PaginatedResult<TagGroupQueryResponse>>> GetAllTagGroups(
        [FromQuery] int tenantId,
        [FromQuery] BaseFilter filter)
    {
        var result = await mediator.Send(new GetAllTagGroupsQuery(tenantId, filter));

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TagGroupQueryResponse>> GetTagGroupById([FromRoute] int id)
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