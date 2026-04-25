using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Repository;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost;Database=RECYCLY_DEV;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true")
            .Options;

        return new AppDbContext(options);
    }
}
