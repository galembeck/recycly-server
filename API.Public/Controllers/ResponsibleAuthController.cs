using API.Public.Controllers._Base;
using API.Public.DTOs;
using API.Public.DTOs.Auth;
using API.Public.DTOs.Responsible;
using API.Public.Filters;
using API.Public.Validators;
using API.Public.Validators.Responsible;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Services.ResponsibleAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("responsible/auth")]
public class ResponsibleAuthController : _BaseController
{
    private readonly IResponsibleAuthService _responsibleAuthService;

    public ResponsibleAuthController(
        IResponsibleAuthService responsibleAuthService,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _responsibleAuthService = responsibleAuthService ?? throw new ArgumentNullException();
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] ResponsibleRegisterDTO body,
        CancellationToken cancellationToken = default)
    {
        await new ResponsibleRegisterValidator().ValidateAndThrowAsync(body);

        var tokens = await _responsibleAuthService.RegisterAsync(
            body.Name, body.Email, body.Cpf, body.Password, body.BirthDate, body.Phones, cancellationToken);

        GenerateAuthCookie(tokens);

        return Ok(AuthResponseDTO.ModelToDTO(tokens));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate(
        [FromBody] AuthenticateDTO body,
        CancellationToken cancellationToken = default)
    {
        await new AuthenticateValidator().ValidateAndThrowAsync(body);

        var tokens = await _responsibleAuthService.AuthenticateAsync(body.Email, body.Password, cancellationToken);

        GenerateAuthCookie(tokens);

        return Ok(AuthResponseDTO.ModelToDTO(tokens));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken = default)
    {
        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        var tokens = await _responsibleAuthService.RefreshAsync(refreshToken, cancellationToken);

        GenerateAuthCookie(tokens);

        return Ok(AuthResponseDTO.ModelToDTO(tokens));
    }

    [ResponsibleAuthAttribute]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeAccessToken(CancellationToken cancellationToken = default)
    {
        var accessToken = Request.Cookies["AccessToken"];
        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        await _responsibleAuthService.RevokeAsync(accessToken, refreshToken, Authenticated.User, cancellationToken);

        RemoveAuthCookie(null);

        return Ok();
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  PASSWORD RECOVERY
    // ─────────────────────────────────────────────────────────────────────────

    [HttpPost("recovery/request")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestPasswordRecovery(
        [FromBody] PasswordRecoveryRequestDTO body,
        CancellationToken cancellationToken = default)
    {
        await new PasswordRecoveryRequestValidator().ValidateAndThrowAsync(body);

        await _responsibleAuthService.SendPasswordRecoveryAsync(body.Email, cancellationToken);
        return Ok(new { message = "Se o e-mail existir em nossa base, você receberá um código de recuperação em instantes." });
    }

    [HttpPost("recovery/verify")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyRecoveryToken(
        [FromBody] PasswordRecoveryVerifyDTO body,
        CancellationToken cancellationToken = default)
    {
        await new PasswordRecoveryVerifyValidator().ValidateAndThrowAsync(body);

        var valid = await _responsibleAuthService.VerifyPasswordRecoveryTokenAsync(body.Email, body.Token, cancellationToken);

        if (!valid)
            return BadRequest(new { message = "Código inválido ou expirado." });

        return Ok(new { message = "Código verificado com sucesso." });
    }

    [HttpPost("recovery/reset")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(
        [FromBody] PasswordResetDTO body,
        CancellationToken cancellationToken = default)
    {
        await new PasswordResetValidator().ValidateAndThrowAsync(body);

        try
        {
            await _responsibleAuthService.ResetPasswordAsync(body.Email, body.Token, body.NewPassword, cancellationToken);
            return Ok(new { message = "Senha redefinida com sucesso. Faça login com sua nova senha." });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
