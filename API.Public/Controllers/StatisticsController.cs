using API.Public.Controllers._Base;
using API.Public.Filters;
using Domain.Enumerators;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController(IStatisticsService statisticsService) : _BaseController
{
    private readonly IStatisticsService _service = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));

    [HttpGet("stats")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> GetStats(CancellationToken cancellationToken = default)
    {
        var stats = await _service.GetStatisticsAsync(Authenticated.User.Id, cancellationToken);
        return Ok(stats);
    }
}
