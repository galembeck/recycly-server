namespace API.Public.DTOs.CooperativeAuth;

public sealed record CooperativePasswordResetDTO
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}
