using System.Diagnostics.CodeAnalysis;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;
using StackExchange.Redis;

namespace Rollout.Lib.Public;

[ExcludeFromCodeCoverage]
public static class RolloutFactory
{
    public static IFeatureManager CreateFeatureManager(IConnectionMultiplexer redis)
    {
        return new FeatureManager(new RedisStorage(redis), new UniformStringToDecimalProvider());
    }

    public static IFeatureManager CreateFeatureManager(
        IFeatureStorage storage,
        IStringToDecimalProvider stringToDecimalProvider
    )
    {
        return new FeatureManager(storage, stringToDecimalProvider);
    }
}
