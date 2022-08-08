using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

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

    public static IServiceCollection AddRolloutWithCustomStorage<T>(this IServiceCollection services)
        where T : class, IFeatureStorage
    {
        services.AddSingleton<IFeatureStorage, T>();
        services.AddSingleton<IFeatureManager>(
            svcs => {
                var storage = svcs.GetRequiredService<IFeatureStorage>();
                return new FeatureManager(storage, new UniformStringToDecimalProvider());
            }
        );
        return services;
    }

    public static IServiceCollection
        AddRolloutWithCustomStorageAndStringToDecimalProvider<TStorage, TStringToDecimalProvider>(
            this IServiceCollection services
        ) where TStorage : class, IFeatureStorage where TStringToDecimalProvider : class, IStringToDecimalProvider
    {
        services.AddSingleton<IFeatureStorage, TStorage>();
        services.AddSingleton<IStringToDecimalProvider, TStringToDecimalProvider>();
        services.AddSingleton<IFeatureManager, FeatureManager>();
        return services;
    }
}
