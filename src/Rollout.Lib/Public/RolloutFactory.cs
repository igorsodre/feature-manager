using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;
using StackExchange.Redis;

namespace Rollout.Lib.Public;

public static class RolloutFactory
{
    public static IFeatureManager GreateFeatureManager(IConnectionMultiplexer redis)
    {
        return new FeatureManager(new RedisStorage(redis), new UniformStringToDecimalProvider());
    }
}
