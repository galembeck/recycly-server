using API.Public.Controllers._Base;
using API.Public.DTOs;
using API.Public.Filters;
using API.Public.Validators;
using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class MaterialController(IMaterialService materialService) : _BaseController
{
    private readonly IMaterialService _materialService = materialService ?? throw new ArgumentNullException(nameof(materialService));

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var materials = await _materialService.GetAllActiveAsync(cancellationToken);
        return Ok(MaterialDTO.ModelToDTO(materials));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
    {
        var material = await _materialService.GetByIdAsync(id, cancellationToken);
        return Ok(MaterialDTO.ModelToDTO(material));
    }

    [HttpPost]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.ADMIN)]
    public async Task<IActionResult> Create([FromBody] MaterialDTO body, CancellationToken cancellationToken = default)
    {
        await new CreateMaterialValidator().ValidateAndThrowAsync(body);

        var material = MaterialDTO.DTOToModel(body)!;
        var saved = await _materialService.CreateMaterialAsync(material, Authenticated.User.Id, cancellationToken);

        return Ok(MaterialDTO.ModelToDTO(saved));
    }

    [HttpPut("{id}")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.ADMIN)]
    public async Task<IActionResult> Update(string id, [FromBody] MaterialDTO body, CancellationToken cancellationToken = default)
    {
        await new CreateMaterialValidator().ValidateAndThrowAsync(body);

        var material = MaterialDTO.DTOToModel(body)!;
        var updated = await _materialService.UpdateMaterialAsync(id, material, Authenticated.User.Id, cancellationToken);

        return Ok(MaterialDTO.ModelToDTO(updated));
    }

    [HttpDelete("{id}")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.ADMIN)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        await _materialService.DeleteMaterialAsync(id, Authenticated.User.Id, cancellationToken);
        return Ok();
    }
}
