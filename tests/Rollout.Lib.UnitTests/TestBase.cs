using Rollout.Lib.Implementations;
using StackExchange.Redis;

namespace Rollout.Lib.UnitTests;

[Collection("SingleRedisInstance")]
public class TestBase : IClassFixture<RedisFixture>, IAsyncLifetime
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

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _fixture.ResetRedis();
    }
}
