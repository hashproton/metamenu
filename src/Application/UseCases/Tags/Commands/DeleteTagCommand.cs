using Application.Services;

namespace Application.UseCases.Tags.Commands;

public record DeleteTagCommand(int TagId) : IRequest;

public class DeleteTagCommandHandler(
    ILogger logger,
    ITagRepository tagRepository) : IRequestHandler<DeleteTagCommand>
{
    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByIdAsync(request.TagId, cancellationToken);
        if (tag is null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId);
        }

        await tagRepository.DeleteAsync(tag, cancellationToken);
    }
}

