namespace Domain.Services;

public record DashboardStats(
    decimal TotalSalesRevenue,
    int TotalCollectsCount,
    decimal TotalSalesProfit,
    List<DashboardChartPoint> ChartData,
    decimal MetalKg,
    decimal PlasticKg,
    decimal GlassKg
);

public record DashboardChartPoint(string Date, int Collects, int Sales);

public interface IDashboardService
{
    Task<DashboardStats> GetStatsAsync(string cooperativeId, CancellationToken cancellationToken = default);
}
