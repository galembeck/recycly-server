namespace API.Public.DTOs.CooperativeAuth;

public sealed record CooperativePasswordRecoveryRequestDTO
{
    public string Email { get; set; }
}
