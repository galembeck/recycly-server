using Domain.Repository;
using System.Globalization;

namespace Domain.Services;

public class StatisticsService(ICollectRepository collectRepository) : IStatisticsService
{
    private readonly ICollectRepository _collectRepository = collectRepository;

    private static readonly string[] DayNames = ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"];

    public async Task<StatisticsData> GetStatisticsAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        var collects = await _collectRepository.GetByCooperativeAsync(cooperativeId, cancellationToken);

        var today = DateTime.UtcNow.Date;
        var ptBR = new CultureInfo("pt-BR");

        var monthlyKg = Enumerable.Range(0, 6)
            .Select(i => today.AddMonths(-5 + i))
            .Select(month => new MonthlyKgPoint(
                Month: char.ToUpper(month.ToString("MMM", ptBR)[0]) + month.ToString("MMM", ptBR)[1..],
                WeightKg: collects
                    .Where(c => c.CollectedAt.Year == month.Year && c.CollectedAt.Month == month.Month)
                    .Sum(c => c.WeightKg)
            ))
            .ToList();

        var materialDistribution = collects
            .Where(c => c.Material != null)
            .GroupBy(c => c.Material!.Name)
            .Select(g => new MaterialKgPoint(g.Key, g.Sum(c => c.WeightKg)))
            .OrderByDescending(m => m.WeightKg)
            .ToList();

        var weeklyActivity = Enumerable.Range(0, 7)
            .Select(i => new WeeklyActivityPoint(
                Day: DayNames[i],
                Collects: collects.Count(c => (int)c.CollectedAt.DayOfWeek == i)
            ))
            .ToList();

        var totalKg = collects.Sum(c => c.WeightKg);
        var achievements = new List<AchievementPoint>
        {
            new("Iniciante Verde",       "Primeira coleta realizada",  collects.Count >= 1),
            new("Reciclador Dedicado",   "10 coletas realizadas",      collects.Count >= 10),
            new("Campeão Ambiental",     "100kg reciclados",           totalKg >= 100),
            new("Mestre da Reciclagem",  "500kg reciclados",           totalKg >= 500),
            new("Guardião do Planeta",   "1.000kg reciclados",         totalKg >= 1000),
        };

        return new StatisticsData(monthlyKg, materialDistribution, weeklyActivity, achievements);
    }
}
