using Microsoft.AspNetCore.Http;

namespace API.Public.DTOs;

public class UpdateProfileDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Cellphone { get; set; }
    public string? Document { get; set; }
    public DateOnly? BirthDate { get; set; }
    public List<string>? Phones { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirmation { get; set; }
}
