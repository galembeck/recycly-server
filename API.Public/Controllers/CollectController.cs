using API.Public.Controllers._Base;
using API.Public.DTOs;
using API.Public.Filters;
using API.Public.Validators;
using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class CollectController(ICollectService collectService) : _BaseController
{
    private readonly ICollectService _service = collectService ?? throw new ArgumentNullException(nameof(collectService));

    [HttpGet]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.CLIENT, ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> GetMyHistory(CancellationToken cancellationToken = default)
    {
        var collects = await _service.GetByUserAsync(Authenticated.User.Id, cancellationToken);
        return Ok(CollectResponseDTO.ModelToDTO(collects));
    }

    [HttpGet("point/{pointId}")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> GetByPoint(string pointId, CancellationToken cancellationToken = default)
    {
        var collects = await _service.GetByCollectionPointAsync(pointId, cancellationToken);
        return Ok(CollectResponseDTO.ModelToDTO(collects));
    }

    [HttpPost]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.CLIENT, ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> Create([FromBody] CreateCollectDTO body, CancellationToken cancellationToken = default)
    {
        await new CreateCollectValidator().ValidateAndThrowAsync(body);

        var collect = new Collect
        {
            CollectionPointId = body.CollectionPointId,
            MaterialId = body.MaterialId,
            UserId = Authenticated.User.Id,
            WeightKg = body.WeightKg,
            CollectedAt = body.CollectedAt,
            Notes = body.Notes,
        };

        var saved = await _service.CreateCollectAsync(collect, Authenticated.User.Id, cancellationToken);

        return Ok(CollectResponseDTO.ModelToDTO(saved));
    }

    [HttpDelete("{id}")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.CLIENT, ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteCollectAsync(id, Authenticated.User.Id, cancellationToken);
        return Ok();
    }
}
