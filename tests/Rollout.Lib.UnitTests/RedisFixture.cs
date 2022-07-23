using Rollout.Lib.UnitTests.Helpers;
using StackExchange.Redis;

namespace Rollout.Lib.UnitTests;

public class RedisFixture
{
    public IConnectionMultiplexer Redis { get; }

    public RedisFixture()
    {
        Redis = ConnectionMultiplexer.Connect(SettingsHelper.GetRedisConnectionString());
    }

    public async Task ResetRedis()
    {
        foreach (var endpoint in Redis.GetEndPoints())
        {
            var server = Redis.GetServer(endpoint);
            await server.FlushAllDatabasesAsync();
        }
    }
}
