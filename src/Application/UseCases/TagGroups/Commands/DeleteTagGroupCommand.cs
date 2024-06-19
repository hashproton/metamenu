using Application.Services;

namespace Application.UseCases.TagGroups.Commands;

public class DeleteTagGroupCommand(
    int id) : IRequest
{
    public int Id { get; set; } = id;
}

public class DeleteTagGroupCommandHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository) : IRequestHandler<DeleteTagGroupCommand>
{
    public async Task Handle(DeleteTagGroupCommand request, CancellationToken cancellationToken)
    {
        var tagGroup = await tagGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tagGroup is null)
        {
            throw new NotFoundException(nameof(TagGroup), request.Id);
        }

        await tagGroupRepository.DeleteAsync(tagGroup, cancellationToken);

        logger.LogInformation($"Tag group {tagGroup.Id} deleted");
    }
}