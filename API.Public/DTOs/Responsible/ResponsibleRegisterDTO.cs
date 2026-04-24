namespace API.Public.DTOs.Responsible;

public sealed record ResponsibleRegisterDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Password { get; set; }
    public DateOnly BirthDate { get; set; }
    public List<string> Phones { get; set; }
}
