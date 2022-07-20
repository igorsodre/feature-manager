using Rollout.Lib.Models;

namespace Rollout.Lib.Interfaces;

internal interface IFeatureStorage
{
    Task<Feature> GetFeature(string featureName);

    Task StoreFeature(Feature feature);

    Task RemoveFeature(string featureName);
}
