namespace Infra.Services.AuthService.Responses;

internal sealed class RawGetMeResponseClaim(
    string type,
    string value)
{
    public string Type { get; set; } = type;

    public string Value { get; set; } = value;
}