using System.Runtime.CompilerServices;
using System.Text.Json;
using Rollout.Lib.Exceptions;
using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;
using StackExchange.Redis;

[assembly: InternalsVisibleTo("Rollout.Lib.UnitTests")]

namespace Rollout.Lib.Implementations;

internal class RedisStorage : IFeatureStorage
{
    private readonly IConnectionMultiplexer _redis;
    private const string KeyPrefix = "___feature___";

    public RedisStorage(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<Feature?> GetFeature(string featureName)
    {
        var db = _redis.GetDatabase();
        var json = await db.StringGetAsync(KeyPrefix + ":" + featureName);
        return json.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Feature>(json!);
    }

    public async Task<IList<Feature>> GetAllFeatures()
    {
        var db = _redis.GetDatabase();
        var endpoints = _redis.GetEndPoints();
        var keys = new List<RedisKey>();
        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            await foreach (var key in server.KeysAsync(pattern: KeyPrefix + "*"))
            {
                key.Append(key);
            }
        }

        var result = await Task.WhenAll(keys.Select(k => db.StringGetAsync(k)));
        return result.Where(r => !r.IsNullOrEmpty)
            .Select(r => JsonSerializer.Deserialize<Feature>(r.ToString()))
            .Where(f => f is not null)
            .ToList()!;
    }

    public async Task StoreFeature(Feature feature)
    {
        if (string.IsNullOrEmpty(feature?.Name))
        {
            throw new NullFeatureName();
        }

        var db = _redis.GetDatabase();
        var stringFeature = JsonSerializer.Serialize(feature);
        await db.StringSetAsync(KeyPrefix + ":" + feature.Name, stringFeature);
    }

    public async Task RemoveFeature(string featureName)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            throw new NullFeatureName("Feature name cannot be null or empty");
        }

        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(KeyPrefix + ":" + featureName);
    }
}
