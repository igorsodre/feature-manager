using Rollout.Lib.Implementations;
using StackExchange.Redis;

namespace Rollout.Lib.UnitTests;

[Collection("SingleRedisInstance")]
public class TestBase : IClassFixture<RedisFixture>, IAsyncDisposable
{
    private readonly RedisFixture _fixture;

    protected IConnectionMultiplexer Redis { get; }

    internal RedisStorage Storage { get; }

    public TestBase(RedisFixture fixture)
    {
        _fixture = fixture;
        Redis = fixture.Redis;
        Storage = new RedisStorage(Redis);
    }

    public async ValueTask DisposeAsync()
    {
        await _fixture.ResetRedis();
    }
}
