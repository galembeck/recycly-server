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
public class SaleController(ISaleService saleService) : _BaseController
{
    private readonly ISaleService _service = saleService ?? throw new ArgumentNullException(nameof(saleService));

    [HttpGet]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken = default)
    {
        var sales = await _service.GetByCooperativeAsync(Authenticated.User.Id, cancellationToken);
        return Ok(SaleResponseDTO.ModelToDTO(sales));
    }

    [HttpPost]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> Create([FromBody] CreateSaleDTO body, CancellationToken cancellationToken = default)
    {
        await new CreateSaleValidator().ValidateAndThrowAsync(body);

        var sale = new Sale
        {
            BuyerName = body.BuyerName,
            WeightKg = body.WeightKg,
            Price = body.Price,
            SoldAt = body.SoldAt,
            Notes = body.Notes,
            CooperativeId = Authenticated.User.Id,
        };

        var saved = await _service.CreateSaleAsync(sale, body.MaterialIds, Authenticated.User.Id, cancellationToken);

        return Ok(SaleResponseDTO.ModelToDTO(saved));
    }

    [HttpDelete("{id}")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteSaleAsync(id, Authenticated.User.Id, cancellationToken);
        return Ok();
    }
}
