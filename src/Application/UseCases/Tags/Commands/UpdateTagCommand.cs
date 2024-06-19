namespace Application.UseCases.Tags.Commands;

public class UpdateTagCommand(
    int tagId) : IRequest
{
    public int TagId { get; set; } = tagId;
    
    public int? TagGroupId { get; set; }

    public string? Name { get; set; }
}

public class UpdateTagCommandHandler(
    ITagGroupRepository tagGroupRepository,
    ITagRepository tagRepository) : IRequestHandler<UpdateTagCommand>
{
    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByIdAsync(request.TagId, cancellationToken);
        if (tag is null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId);
        }

        if (request.TagGroupId.HasValue)
        {
            var tagGroup = await tagGroupRepository.GetByIdAsync(request.TagGroupId.Value, cancellationToken);
            if (tagGroup is null)
            {
                throw new NotFoundException(nameof(TagGroup), request.TagGroupId.Value);
            }

            tag.TagGroup = tagGroup;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var existingTag = await tagRepository.GetTagByNameAsync(tag.TagGroupId, request.Name, cancellationToken);

            if (existingTag is not null)
            {
                if (existingTag.Id != tag.Id)
                {
                    throw new ConflictException(
                        $"A tag with the name '{request.Name}' already exists in the tag group with Name '{existingTag.TagGroup.Name}'.");
                }
            }

            tag.Name = request.Name;
        }

        await tagRepository.UpdateAsync(tag, cancellationToken);
    }
}