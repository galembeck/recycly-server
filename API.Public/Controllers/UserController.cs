using API.Public.Controllers._Base;
using API.Public.DTOs;
using API.Public.Filters;
using API.Public.Validators;
using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService, IFileStorageService fileStorageService) : _BaseController
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    private readonly IFileStorageService _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));

    [HttpPost]
    [AllowAnonymous]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Register([FromForm] PrivateUserDTO body, IFormFile? document)
    {
        var securityInfo = GetSecurityInfo(Request);

        await new UserCreationValidator().ValidateAndThrowAsync(body);

        var user = PrivateUserDTO.DTOToModel(body)!;

        if (document != null)
        {
            using var stream = document.OpenReadStream();
            var relativePath = await _fileStorageService.UploadFileAsync(stream, document.FileName, "documents");
            user.DocumentUrl = _fileStorageService.GetFileUrl(relativePath);
        }

        var model = await _userService.CreateAsync(user, securityInfo);

        return Ok(PublicUserDTO.ModelToDTO(model));
    }

    [AuthAttribute]
    [Filters.Authorize(ProfileType.CLIENT, ProfileType.ADMIN)]
    [HttpGet]
    public async Task<IActionResult> Me(CancellationToken cancellationToken = default)
    {
        var securityInfo = base.GetSecurityInfo(Request);

        User response = await _userService.GetUserAsync(Authenticated.User.Id, securityInfo, cancellationToken);

        return Ok(PublicUserDTO.ModelToDTO(response));
    }

    [HttpPut("me")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.CLIENT, ProfileType.ADMIN)]
    [ProducesResponseType(typeof(PublicUserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMe([FromForm] UpdateProfileDTO dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = Authenticated?.User?.Id;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                if (dto.Password != dto.PasswordConfirmation)
                    return StatusCode(StatusCodes.Status400BadRequest, "Passwords do not match.");

                if (dto.Password.Length < 8)
                    return StatusCode(StatusCodes.Status400BadRequest, "Password must be at least 8 characters.");
            }

            var user = await _userService.UpdateProfileAsync(
                userId,
                dto.Name,
                dto.Email,
                dto.Document,
                dto.BirthDate,
                dto.Phones,
                dto.Password,
                cancellationToken);

            return Ok(PublicUserDTO.ModelToDTO(user));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
