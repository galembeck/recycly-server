namespace API.Public.DTOs.CooperativeAuth;

public sealed record CooperativePasswordRecoveryVerifyDTO
{
    public string Email { get; set; }
    public string Token { get; set; }
}
