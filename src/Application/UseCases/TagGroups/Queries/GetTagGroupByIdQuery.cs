using Application.Services;
using Application.UseCases.TagGroups.Queries.Common;

namespace Application.UseCases.TagGroups.Queries;

public record GetTagGroupByIdQuery(
    int Id) : IRequest<Result<TagGroupQueryResponse>>;

public class GetTagGroupByIdQueryHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository) : IRequestHandler<GetTagGroupByIdQuery, Result<TagGroupQueryResponse>>
{
    public async Task<Result<TagGroupQueryResponse>> Handle(
        GetTagGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tagGroup = await tagGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tagGroup is null)
        {
            return Result.Failure<TagGroupQueryResponse>(TagGroupErrors.TagGroupNotFound);
        }

        logger.LogInformation($"Tag group {tagGroup.Id} retrieved");

        return Result.Success(tagGroup.ToQueryResponse());
    }
}
