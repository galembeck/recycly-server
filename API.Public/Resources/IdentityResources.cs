using Domain.Data.Entities;
using System.Security.Claims;

namespace API.Public.Resources;

public static class IdentityResources
{
    public static void AddUserOnThread(User user)
    {
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims, "Auth", "Id", ClaimTypes.Role);
        var principal = new IdentityPrincipal(identity, user);

        Thread.CurrentPrincipal = principal;
    }

    public static void AddCooperativeOnThread(Cooperative cooperative)
    {
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims, "CooperativeAuth", "Id", ClaimTypes.Role);
        var principal = new IdentityPrincipal(identity, cooperative);

        Thread.CurrentPrincipal = principal;
    }
}
