using Application.Services;

namespace Application.UseCases.TagGroups.Commands;

public class DeleteTagGroupCommand(
    int id) : IRequest<Result>
{
    public int Id { get; set; } = id;
}

public class DeleteTagGroupCommandHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository) : IRequestHandler<DeleteTagGroupCommand, Result>
{
    public async Task<Result> Handle(DeleteTagGroupCommand request, CancellationToken cancellationToken)
    {
        var tagGroup = await tagGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tagGroup is null)
        {
            return Result.Failure(TagGroupErrors.TagGroupNotFound);
        }

        await tagGroupRepository.DeleteAsync(tagGroup, cancellationToken);

        logger.LogInformation($"Tag group {tagGroup.Id} deleted");

        return Result.Success();
    }
}