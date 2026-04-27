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
public class CollectionPointController(ICollectionPointService collectionPointService) : _BaseController
{
    private readonly ICollectionPointService _service = collectionPointService ?? throw new ArgumentNullException(nameof(collectionPointService));

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var points = await _service.GetAllPublicAsync(cancellationToken);
        return Ok(CollectionPointResponseDTO.ModelToDTO(points));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
    {
        var point = await _service.GetWithMaterialsAsync(id, cancellationToken);
        return Ok(CollectionPointResponseDTO.ModelToDTO(point));
    }

    [HttpGet("mine")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken = default)
    {
        var points = await _service.GetByCooperativeAsync(Authenticated.User.Id, cancellationToken);
        return Ok(CollectionPointResponseDTO.ModelToDTO(points));
    }

    [HttpPost]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> Create([FromBody] CreateCollectionPointDTO body, CancellationToken cancellationToken = default)
    {
        await new CreateCollectionPointValidator().ValidateAndThrowAsync(body);

        var entity = new CollectionPoint
        {
            Name = body.Name,
            Description = body.Description,
            ZipCode = body.ZipCode,
            Address = body.Address,
            Number = body.Number,
            Complement = body.Complement,
            Neighborhood = body.Neighborhood,
            City = body.City,
            State = body.State,
            OpeningTime = TimeOnly.Parse(body.OpeningTime),
            ClosingTime = TimeOnly.Parse(body.ClosingTime),
            Phone = body.Phone,
            CooperativeId = Authenticated.User.Id,
        };

        var saved = await _service.CreateWithGeocodingAsync(entity, body.MaterialIds, Authenticated.User.Id, cancellationToken);

        return Ok(CollectionPointResponseDTO.ModelToDTO(saved));
    }

    [HttpPut("{id}")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> Update(string id, [FromBody] CreateCollectionPointDTO body, CancellationToken cancellationToken = default)
    {
        await new CreateCollectionPointValidator().ValidateAndThrowAsync(body);

        var entity = new CollectionPoint
        {
            Name = body.Name,
            Description = body.Description,
            ZipCode = body.ZipCode,
            Address = body.Address,
            Number = body.Number,
            Complement = body.Complement,
            Neighborhood = body.Neighborhood,
            City = body.City,
            State = body.State,
            OpeningTime = TimeOnly.Parse(body.OpeningTime),
            ClosingTime = TimeOnly.Parse(body.ClosingTime),
            Phone = body.Phone,
        };

        var updated = await _service.UpdateCollectionPointAsync(id, entity, Authenticated.User.Id, cancellationToken);

        return Ok(CollectionPointResponseDTO.ModelToDTO(updated));
    }

    [HttpPut("{id}/materials")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> UpdateMaterials(string id, [FromBody] UpdateMaterialsDTO body, CancellationToken cancellationToken = default)
    {
        await _service.UpdateMaterialsAsync(id, body.MaterialIds, Authenticated.User.Id, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteCollectionPointAsync(id, Authenticated.User.Id, cancellationToken);
        return Ok();
    }
}
