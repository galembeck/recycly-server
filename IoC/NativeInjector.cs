using Domain.Repository;
using Domain.Repository.User;
using Domain.Services;
using Domain.Services.Geocoding;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repository;
using Repository.Repository.User;

namespace IoC;

public static class NativeInjector
{
    public static void ConfigureInjections(this IServiceCollection services)
    {
        #region .: INTERNAL INJECTIONS :.

        #region .: USER :.
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        #endregion .: USER :.

        #region .: USER SECURITY INFO :.

        services.AddScoped<IUserSecurityInfoRepository, UserSecurityInfoRepository>();

        #endregion .: USER SECURITY INFO :.

        #region .: USER HISTORIC :.

        services.AddScoped<IUserHistoricRepository, UserHistoricRepository>();
        services.AddScoped<IUserHistoricService, UserHistoricService>();

        #endregion .: USER HISTORIC :.

        #region .: AUTH :.

        services.AddScoped<IAuthService, AuthService>();

        #endregion .: AUTH :.

        #region .: TOKENS :.

        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        #endregion .: TOKENS :.

        #region .: FILE STORAGE :.

        services.AddScoped<IFileStorageService, FileStorageService>();

        #endregion .: FILE STORAGE :.

        #region .: MATERIAL :.

        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IMaterialService, MaterialService>();

        #endregion .: MATERIAL :.

        #region .: COLLECTION POINT :.

        services.AddScoped<ICollectionPointRepository, CollectionPointRepository>();
        services.AddScoped<ICollectionPointService, CollectionPointService>();

        #endregion .: COLLECTION POINT :.

        #region .: COLLECT :.

        services.AddScoped<ICollectRepository, CollectRepository>();
        services.AddScoped<ICollectService, CollectService>();

        #endregion .: COLLECT :.

        #region .: SALE :.

        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ISaleService, SaleService>();

        #endregion .: SALE :.

        #region .: DASHBOARD :.

        services.AddScoped<IDashboardService, DashboardService>();

        #endregion .: DASHBOARD :.

        #region .: STATISTICS :.

        services.AddScoped<IStatisticsService, StatisticsService>();

        #endregion .: STATISTICS :.

        #region .: GEOCODING :.

        services.AddScoped<IGeocodingService, GeocodingService>();

        #endregion .: GEOCODING :.

        // #region .: EMAIL :.
        // services.AddScoped<IEmailService, EmailService>();
        // #endregion .: EMAIL :.

        #endregion .: INTERNAL INJECTIONS :.
    }
}
