namespace Application.Services.AuthService.Responses;

public class ErrorsResponse(
    List<Error> errors)
{
    public List<Error> Errors { get; set; } = errors;
}