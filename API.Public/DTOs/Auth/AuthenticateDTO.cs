namespace API.Public.DTOs;

public sealed record AuthenticateDTO
{
    public string Identifier { get; set; }
    public string Password { get; set; }
}
