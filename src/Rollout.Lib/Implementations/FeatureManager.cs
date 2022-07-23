using Rollout.Lib.Interfaces;

namespace Rollout.Lib.Implementations;

class FeatureManager : IFeatureManager
{
    public Task SetPercentage(string featureName, decimal percentage)
    {
        throw new NotImplementedException();
    }

    public Task SetGroups(string featureName, IList<string> groups)
    {
        throw new NotImplementedException();
    }

    public Task RemoveGroups(string featureName, IList<string> groups)
    {
        throw new NotImplementedException();
    }

    public Task SetUsers(string featureName, IList<string> users)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUsers(string featureName, IList<string> users)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsActiveFor(string featureName, string? user = null, string? group = null)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetAllFeatures()
    {
        throw new NotImplementedException();
    }

    public Task Deactivate(string featureName)
    {
        throw new NotImplementedException();
    }
}
