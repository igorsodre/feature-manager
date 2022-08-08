using Rollout.Lib.Models;

namespace Rollout.Lib.Interfaces;

public interface IFeatureStorage
{
    Task<Feature?> GetFeature(string featureName);

    Task<IList<Feature>> GetAllFeatures();

    Task StoreFeature(Feature feature);

    Task RemoveFeature(string featureName);
}
