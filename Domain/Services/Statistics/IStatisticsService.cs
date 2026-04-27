namespace Domain.Services;

public record StatisticsData(
    List<MonthlyKgPoint> MonthlyKg,
    List<MaterialKgPoint> MaterialDistribution,
    List<WeeklyActivityPoint> WeeklyActivity,
    List<AchievementPoint> Achievements
);

public record MonthlyKgPoint(string Month, decimal WeightKg);
public record MaterialKgPoint(string Material, decimal WeightKg);
public record WeeklyActivityPoint(string Day, int Collects);
public record AchievementPoint(string Title, string Description, bool Unlocked);

public interface IStatisticsService
{
    Task<StatisticsData> GetStatisticsAsync(string cooperativeId, CancellationToken cancellationToken = default);
}
