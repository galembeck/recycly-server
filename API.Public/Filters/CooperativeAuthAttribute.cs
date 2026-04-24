using API.Public.Resources;
using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository.Cooperative;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Public.Filters;

public class CooperativeAuthAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var req = httpContext.Request;

        var token = req.Cookies["Cooperative_AccessToken"];

        if (string.IsNullOrEmpty(token))
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        ICooperativeAccessTokenRepository accessTokenRepository = httpContext.RequestServices
            .GetRequiredService<ICooperativeAccessTokenRepository>()
            ?? throw new ArgumentNullException(nameof(accessTokenRepository), "ICooperativeAccessTokenRepository service is not registered in the dependency injection container");

        ICooperativeRepository cooperativeRepository = httpContext.RequestServices
            .GetRequiredService<ICooperativeRepository>()
            ?? throw new ArgumentNullException(nameof(cooperativeRepository), "ICooperativeRepository service is not registered in the dependency injection container");

        CooperativeAccessToken? accessToken;

        try
        {
            accessToken = await accessTokenRepository.GetByTokenAsync(token);
        }
        catch (PersistenceException)
        {
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);
        }

        if (accessToken is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        if (accessToken.ExpiresAt < DateTimeOffset.UtcNow)
            throw new AuthenticationException(AuthenticationErrorMessage.TOKEN_EXPIRED);

        Domain.Data.Entities.Cooperative? cooperative = await cooperativeRepository.GetAsync(accessToken.CooperativeId);

        if (cooperative is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);
        else
            IdentityResources.AddCooperativeOnThread(cooperative);

        await next();
    }
}
