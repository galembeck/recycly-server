using API.Public.Controllers._Base;
using API.Public.Filters;
using Domain.Enumerators;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class DashboardController(IDashboardService dashboardService) : _BaseController
{
    private readonly IDashboardService _service = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));

    [HttpGet("stats")]
    [AuthAttribute]
    [Filters.Authorize(ProfileType.COOPERATIVE, ProfileType.ADMIN)]
    public async Task<IActionResult> GetStats(CancellationToken cancellationToken = default)
    {
        var stats = await _service.GetStatsAsync(Authenticated.User.Id, cancellationToken);
        return Ok(stats);
    }
}
