namespace API.Public.DTOs.CooperativeAuth;

public sealed record CooperativeRegisterDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Password { get; set; }
    public DateOnly BirthDate { get; set; }
    public List<string> Phones { get; set; }
}
