using Application.Services;

namespace Application.UseCases.Tags.Commands;

public record CreateTagCommand(
    int TagGroupId,
    string Name) : IRequest<int>;

public class CreateTagCommandHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository,
    ITagRepository tagRepository) : IRequestHandler<CreateTagCommand, int>
{
    public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tagGroup = await tagGroupRepository.GetByIdAsync(request.TagGroupId, cancellationToken);
        if (tagGroup is null)
        {
            throw new NotFoundException(nameof(TagGroup), request.TagGroupId);
        }

        var existingTag = await tagRepository.GetTagByNameAsync(
            request.TagGroupId,
            request.Name,
            cancellationToken);

        if (existingTag is not null)
        {
            throw new ConflictException(
                $"Tag with name {request.Name} already exists in TagGroup {request.TagGroupId}");
        }

        var tag = new Tag
        {
            Name = request.Name,
            TagGroupId = request.TagGroupId
        };

        await tagRepository.AddAsync(tag, cancellationToken);

        logger.LogInformation($"Tag {tag.Id} created in TagGroup {tag.TagGroupId}");

        return tag.Id;
    }
}