using Domain.Data.Entities;
using System.Security.Principal;

namespace API.Public.Resources;

public class IdentityPrincipal : IPrincipal
{
    public User? User { get; }
    public Cooperative? Cooperative { get; }
    public IIdentity Identity { get; }

    public IdentityPrincipal(IIdentity identity, User user)
    {
        Identity = identity;
        User = user;
    }

    public IdentityPrincipal(IIdentity identity, Cooperative cooperative)
    {
        Identity = identity;
        Cooperative = cooperative;
    }

    public bool IsInRole(string role) => true;
}
