using Application.Services;

namespace Application.UseCases.TagGroups.Commands;

public class UpdateTagGroupCommand(
    int id) : IRequest
{
    public int Id { get; set; } = id;

    public string? Name { get; set; }
}

public class UpdateTagGroupCommandHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository) : IRequestHandler<UpdateTagGroupCommand>
{
    public async Task Handle(UpdateTagGroupCommand request, CancellationToken cancellationToken)
    {
        var tagGroup = await tagGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tagGroup is null)
        {
            throw new NotFoundException(nameof(TagGroup), request.Id);
        }
        
        var existingTagGroup = await tagGroupRepository.GetTagGroupByNameAsync(tagGroup.TenantId, request.Name!, cancellationToken);
        if (existingTagGroup is not null)
        {
            throw new ConflictException("Tag Group with the same name for the tenant already exists");
        }

        tagGroup.Name = request.Name;

        await tagGroupRepository.UpdateAsync(tagGroup, cancellationToken);

        logger.LogInformation($"Tag group {tagGroup.Id} updated");
    }
}