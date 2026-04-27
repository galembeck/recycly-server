using API.Public.Configuration;
using AspNetCoreRateLimit;
using Domain.Constants;
using IoC;
using Resend;

namespace API.Public.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        services.ConfigureRateLimit(configuration);
        services.ConfigureDatabase(configuration);
        services.ConfigureJwt();
        //services.ConfigureLogger(configuration);
        services.AddCoreMemoryCache(configuration);

        services.AddHttpContextAccessor();

        services.ConfigureHangfire(configuration);
        // services.ConfigureResend();
        services.ConfigureInjections();
        services.AddSignalR();
        services.AddAuthorization();
        services.AddResponseCompression();
        services.ConfigureCompression();
        services.ConfigureControllers();
        services.AddOpenApi();

        const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins, policy =>
            {
                policy.WithOrigins(
                        "http://localhost:5173",
                        "http://localhost:5174")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.Secure = CookieSecurePolicy.Always;
        });

        services.AddHttpClient("Nominatim", client =>
        {
            client.BaseAddress = new Uri("https://nominatim.openstreetmap.org");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Recycly/1.0 (contact@recycly.com)");
            client.Timeout = TimeSpan.FromSeconds(10);
        });
    }

    private static void ConfigureResend(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddHttpClient<ResendClient>();
        services.Configure<ResendClientOptions>(o =>
        {
            o.ApiToken = Constant.Settings.EmailServiceSettings.ApiToken;
        });
        services.AddTransient<IResend, ResendClient>();
    }
}
