using Rollout.Lib.Models;

namespace Rollout.Lib.Interfaces;

public interface IFeatureManager
{
    Task SetPercentage(string featureName, decimal percentage);

    Task SetGroups(string featureName, IList<string> groups);

    Task RemoveGroups(string featureName, IList<string> groups);

    Task SetUsers(string featureName, IList<string> users);

    Task RemoveUsers(string featureName, IList<string> users);

    Task<bool> IsActiveFor(string featureName, string? user = null, string? group = null);

    Task<Feature?> GetFeature(string featureName);

    Task<IList<Feature>> GetAllFeatures();

    Task Deactivate(string featureName);
}
