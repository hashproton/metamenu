namespace Infra.Services.AuthService.Responses;

internal sealed class RawGetMeResponse(
    Guid userId,
    string email,
    List<string> roles,
    List<RawGetMeResponseClaim> claims)
{
    public Guid UserId { get; set; } = userId;

    public string Email { get; set; } = email;

    public List<string> Roles { get; set; } = roles;

    public List<RawGetMeResponseClaim> Claims { get; set; } = claims;
}