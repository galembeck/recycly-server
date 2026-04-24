using API.Public.Controllers._Base;
using API.Public.DTOs.CooperativeAuth;
using API.Public.Filters;
using API.Public.Validators.CooperativeAuth;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Services.CooperativeAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("cooperative/auth")]
public class CooperativeAuthController : _BaseController
{
    private readonly ICooperativeAuthService _cooperativeAuthService;

    public CooperativeAuthController(
        ICooperativeAuthService cooperativeAuthService,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _cooperativeAuthService = cooperativeAuthService ?? throw new ArgumentNullException();
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] CooperativeRegisterDTO body,
        CancellationToken cancellationToken = default)
    {
        await new CooperativeRegisterValidator().ValidateAndThrowAsync(body);

        var tokens = await _cooperativeAuthService.RegisterAsync(
            body.Name, body.Email, body.Cpf, body.Password, body.BirthDate, body.Phones, cancellationToken);

        GenerateCooperativeAuthCookie(tokens);

        return Ok(CooperativeAuthResponseDTO.ModelToDTO(tokens));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate(
        [FromBody] CooperativeAuthenticateDTO body,
        CancellationToken cancellationToken = default)
    {
        await new CooperativeAuthenticateValidator().ValidateAndThrowAsync(body);

        var tokens = await _cooperativeAuthService.AuthenticateAsync(body.Email, body.Password, cancellationToken);

        GenerateCooperativeAuthCookie(tokens);

        return Ok(CooperativeAuthResponseDTO.ModelToDTO(tokens));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken = default)
    {
        var refreshToken = Request.Cookies["Cooperative_RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        var tokens = await _cooperativeAuthService.RefreshAsync(refreshToken, cancellationToken);

        GenerateCooperativeAuthCookie(tokens);

        return Ok(CooperativeAuthResponseDTO.ModelToDTO(tokens));
    }

    [CooperativeAuthAttribute]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeAccessToken(CancellationToken cancellationToken = default)
    {
        var accessToken = Request.Cookies["Cooperative_AccessToken"];
        var refreshToken = Request.Cookies["Cooperative_RefreshToken"];

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        await _cooperativeAuthService.RevokeAsync(accessToken, refreshToken, Authenticated.Cooperative, cancellationToken);

        RemoveCooperativeAuthCookie();

        return Ok();
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  PASSWORD RECOVERY
    // ─────────────────────────────────────────────────────────────────────────

    [HttpPost("recovery/request")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestPasswordRecovery(
        [FromBody] CooperativePasswordRecoveryRequestDTO body,
        CancellationToken cancellationToken = default)
    {
        await new CooperativePasswordRecoveryRequestValidator().ValidateAndThrowAsync(body);

        await _cooperativeAuthService.SendPasswordRecoveryAsync(body.Email, cancellationToken);
        return Ok(new { message = "Se o e-mail existir em nossa base, você receberá um código de recuperação em instantes." });
    }

    [HttpPost("recovery/verify")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyRecoveryToken(
        [FromBody] CooperativePasswordRecoveryVerifyDTO body,
        CancellationToken cancellationToken = default)
    {
        await new CooperativePasswordRecoveryVerifyValidator().ValidateAndThrowAsync(body);

        var valid = await _cooperativeAuthService.VerifyPasswordRecoveryTokenAsync(body.Email, body.Token, cancellationToken);

        if (!valid)
            return BadRequest(new { message = "Código inválido ou expirado." });

        return Ok(new { message = "Código verificado com sucesso." });
    }

    [HttpPost("recovery/reset")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(
        [FromBody] CooperativePasswordResetDTO body,
        CancellationToken cancellationToken = default)
    {
        await new CooperativePasswordResetValidator().ValidateAndThrowAsync(body);

        try
        {
            await _cooperativeAuthService.ResetPasswordAsync(body.Email, body.Token, body.NewPassword, cancellationToken);
            return Ok(new { message = "Senha redefinida com sucesso. Faça login com sua nova senha." });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
