using Application.Services;

namespace Application.UseCases.TagGroups.Commands;

public class CreateTagGroupCommand(
    string name,
    int tenantId) : IRequest<int>
{
    public string Name { get; set; } = name;

    public int TenantId { get; set; } = tenantId;
}

public class CreateTagGroupCommandHandler(
    ILogger logger,
    ITagGroupRepository tagGroupRepository,
    ITenantRepository tenantRepository) : IRequestHandler<CreateTagGroupCommand, int>
{
    public async Task<int> Handle(CreateTagGroupCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            throw new NotFoundException(nameof(Tenant), request.TenantId);
        }

        var existingTagGroup =
            await tagGroupRepository.GetTagGroupByNameAsync(request.TenantId, request.Name, cancellationToken);
        if (existingTagGroup is not null)
        {
            throw new ConflictException("Tag group with the same name for the tenant already exists");
        }

        var tagGroup = new TagGroup
        {
            Name = request.Name,
            Tenant = tenant
        };

        await tagGroupRepository.AddAsync(tagGroup, cancellationToken);

        logger.LogInformation($"Tag group {tagGroup.Id} created");

        return tagGroup.Id;
    }
}