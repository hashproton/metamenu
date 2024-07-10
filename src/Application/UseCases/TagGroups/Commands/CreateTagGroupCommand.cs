using Application.Services;

namespace Application.UseCases.TagGroups.Commands;

public class CreateTagGroupCommand(
    string name,
    int tenantId) : IRequest<Result<int>>
{
    public string Name { get; set; } = name;

    public int TenantId { get; set; } = tenantId;
}

public class CreateTagGroupCommandHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository,
    ITenantRepository tenantRepository) : IRequestHandler<CreateTagGroupCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateTagGroupCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return Result.Failure<int>(TenantErrors.TenantNotFound);
        }

        var existingTagGroup =
            await tagGroupRepository.GetTagGroupByNameAsync(request.TenantId, request.Name, cancellationToken);
        if (existingTagGroup is not null)
        {
            return Result.Failure<int>(TagGroupErrors.TagGroupAlreadyExists);
        }

        var tagGroup = new TagGroup
        {
            Name = request.Name,
            Tenant = tenant
        };

        await tagGroupRepository.AddAsync(tagGroup, cancellationToken);

        logger.LogInformation($"Tag group {tagGroup.Id} created");

        return Result.Success(tagGroup.Id);
    }
}