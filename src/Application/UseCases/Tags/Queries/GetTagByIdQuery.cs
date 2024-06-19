using Application.Services;

namespace Application.UseCases.Tags.Queries;

public record GetTagByIdQuery(int Id) : IRequest<GetTagByIdQueryResponse>;

public class GetTagByIdQueryHandler(
    ILogger logger,
    ITagRepository tagRepository) : IRequestHandler<GetTagByIdQuery, GetTagByIdQueryResponse>
{
    public async Task<GetTagByIdQueryResponse> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByIdAsync(request.Id, cancellationToken);

        if (tag is null)
        {
            throw new NotFoundException(nameof(Tag), request.Id);
        }

        logger.LogInformation($"Retrieved Tag with ID {tag.Id}.");

        return new GetTagByIdQueryResponse(tag.Id, tag.Name, tag.TagGroup.Id, tag.TagGroup.Name);
    }
}

public record GetTagByIdQueryResponse(
    int Id,
    string Name,
    int TagGroupId,
    string TagGroupName);