using Application.Services;

namespace Application.UseCases.TagGroups.Queries;

public record GetTagGroupByIdQuery(
    int Id) : IRequest<GetTagGroupByIdQueryResponse>;

public class GetTagGroupByIdQueryHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository) : IRequestHandler<GetTagGroupByIdQuery, GetTagGroupByIdQueryResponse>
{
    public async Task<GetTagGroupByIdQueryResponse> Handle(
        GetTagGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tagGroup = await tagGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tagGroup is null)
        {
            throw new NotFoundException(nameof(TagGroup), request.Id);
        }

        logger.LogInformation($"Tag group {tagGroup.Id} retrieved");

        return new GetTagGroupByIdQueryResponse(tagGroup.Id, tagGroup.Name, tagGroup.TenantId);
    }
}

public record GetTagGroupByIdQueryResponse(
    int Id,
    string Name,
    int TenantId);