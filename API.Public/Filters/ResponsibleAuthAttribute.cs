using API.Public.Resources;
using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository.Responsible;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Public.Filters;

public class ResponsibleAuthAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var req = httpContext.Request;

        var token = req.Cookies["Responsible_AccessToken"];

        if (string.IsNullOrEmpty(token))
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        IResponsibleAccessTokenRepository accessTokenRepository = httpContext.RequestServices
            .GetRequiredService<IResponsibleAccessTokenRepository>()
            ?? throw new ArgumentNullException(nameof(accessTokenRepository), "IResponsibleAccessTokenRepository is not registered");

        IResponsibleRepository responsibleRepository = httpContext.RequestServices
            .GetRequiredService<IResponsibleRepository>()
            ?? throw new ArgumentNullException(nameof(responsibleRepository), "IResponsibleRepository is not registered");

        ResponsibleAccessToken? accessToken;

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

        Domain.Data.Entities.Responsible? responsible = await responsibleRepository.GetAsync(accessToken.ResponsibleId);

        if (responsible is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);
        else
            IdentityResources.AddResponsibleOnThread(responsible);

        await next();
    }
}
