using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Rollout.Lib.UnitTests.Helpers;
using StackExchange.Redis;

namespace Rollout.Lib.UnitTests;

public class RedisFixture : IAsyncLifetime
{
    private IConnectionMultiplexer? _redis;

    public IConnectionMultiplexer Redis
    {
        get { return _redis ??= ConnectionMultiplexer.Connect($"{_container.ConnectionString},allowAdmin=true"); }
    }

    private readonly RedisTestcontainer _container = new TestcontainersBuilder<RedisTestcontainer>()
        .WithDatabase(new RedisTestcontainerConfiguration())
        .Build();

    public async Task DisposeRedis()
    {
        foreach (var endpoint in Redis.GetEndPoints())
        {
            var server = Redis.GetServer(endpoint);
            await server.FlushAllDatabasesAsync();
        }
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
