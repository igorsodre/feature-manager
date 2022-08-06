using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;
using Rollout.Lib.UI.Common;

namespace Rollout.Lib.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRollout(this IServiceCollection services)
    {
        services.AddSingleton<IFeatureStorage, RedisStorage>();
        services.AddSingleton<IFeatureManager>(
            svcs => {
                var storage = svcs.GetRequiredService<IFeatureStorage>();
                return new FeatureManager(storage, new UniformStringToDecimalProvider());
            }
        );
        return services;
    }

    public static IServiceCollection AddRolloutUi(this IServiceCollection services)
    {
        services.ConfigureOptions(typeof(UiConfigureOptions));
        services.AddRazorPages();
        services.AddScoped<ViewRender>();
        return services;
    }
}
