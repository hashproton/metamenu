using Application.Services;

namespace Application.UseCases.TagGroups.Commands;

public class UpdateTagGroupCommand(
    int id) : IRequest<Result>
{
    public int Id { get; set; } = id;

    public string? Name { get; set; }
}

public class UpdateTagGroupCommandHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository) : IRequestHandler<UpdateTagGroupCommand, Result>
{
    public async Task<Result> Handle(UpdateTagGroupCommand request, CancellationToken cancellationToken)
    {
        var tagGroup = await tagGroupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (tagGroup is null)
        {
            return Result.Failure(TagGroupErrors.TagGroupNotFound);
        }

        var existingTagGroup = await tagGroupRepository.GetTagGroupByNameAsync(tagGroup.TenantId, request.Name!, cancellationToken);
        if (existingTagGroup is not null)
        {
            return Result.Failure(TagGroupErrors.TagGroupAlreadyExists);
        }

        tagGroup.Name = request.Name;

        await tagGroupRepository.UpdateAsync(tagGroup, cancellationToken);

        logger.LogInformation($"Tag group {tagGroup.Id} updated");
        
        return Result.Success();
    }
}