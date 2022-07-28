using Microsoft.Extensions.DependencyInjection;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

namespace Rollout.Lib.Extensions;

public static class Services
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
}
