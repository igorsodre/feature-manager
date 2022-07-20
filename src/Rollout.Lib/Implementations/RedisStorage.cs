using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;

namespace Rollout.Lib.Implementations;

internal class RedisStorage : IFeatureStorage
{
    public Task<Feature> GetFeature(string featureName)
    {
        throw new NotImplementedException();
    }

    public Task StoreFeature(Feature feature)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFeature(string featureName)
    {
        throw new NotImplementedException();
    }
}
