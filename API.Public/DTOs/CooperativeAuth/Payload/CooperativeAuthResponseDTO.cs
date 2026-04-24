using Domain.Data.Models.Auth;

namespace API.Public.DTOs.CooperativeAuth;

public class CooperativeAuthResponseDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public CooperativeAuthResponseDTO(Tokens o)
    {
        if (o == null) return;

        AccessToken = o.AccessToken;
        RefreshToken = o.RefreshToken;
    }

    public static CooperativeAuthResponseDTO ModelToDTO(Tokens o)
        => o is null ? null : new CooperativeAuthResponseDTO(o);
}
