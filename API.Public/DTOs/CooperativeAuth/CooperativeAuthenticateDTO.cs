namespace API.Public.DTOs.CooperativeAuth;

public sealed record CooperativeAuthenticateDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}
