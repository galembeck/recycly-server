using Domain.Repository;

namespace Domain.Services;

public class DashboardService(
    ICollectRepository collectRepository,
    ISaleRepository saleRepository) : IDashboardService
{
    private readonly ICollectRepository _collectRepository = collectRepository;
    private readonly ISaleRepository _saleRepository = saleRepository;

    public async Task<DashboardStats> GetStatsAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        var collects = await _collectRepository.GetByCooperativeAsync(cooperativeId, cancellationToken);
        var sales = await _saleRepository.GetByCooperativeAsync(cooperativeId, cancellationToken);

        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-89);

        var chartData = Enumerable.Range(0, 90)
            .Select(i => startDate.AddDays(i))
            .Select(date => new DashboardChartPoint(
                Date: date.ToString("yyyy-MM-dd"),
                Collects: collects.Count(c => c.CollectedAt.Date == date),
                Sales: sales.Count(s => s.SoldAt.Date == date)
            ))
            .ToList();

        var metalKg = collects
            .Where(c => Contains(c.Material?.Name, "metal"))
            .Sum(c => c.WeightKg);

        var plasticKg = collects
            .Where(c => Contains(c.Material?.Name, "plástico") || Contains(c.Material?.Name, "plastico"))
            .Sum(c => c.WeightKg);

        var glassKg = collects
            .Where(c => Contains(c.Material?.Name, "vidro"))
            .Sum(c => c.WeightKg);

        return new DashboardStats(
            TotalSalesRevenue: sales.Sum(s => s.Price),
            TotalCollectsCount: collects.Count,
            TotalSalesProfit: sales.Sum(s => s.Price),
            ChartData: chartData,
            MetalKg: metalKg,
            PlasticKg: plasticKg,
            GlassKg: glassKg
        );
    }

    private static bool Contains(string? name, string keyword) =>
        name?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true;
}
