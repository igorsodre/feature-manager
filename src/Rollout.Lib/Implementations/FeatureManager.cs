using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;

namespace Rollout.Lib.Implementations;

internal class FeatureManager : IFeatureManager
{
    private readonly IFeatureStorage _featureStorage;
    private readonly IStringToDecimalProvider _stringToDecimalProvider;

    public FeatureManager(IFeatureStorage featureStorage, IStringToDecimalProvider stringToDecimalProvider)
    {
        _featureStorage = featureStorage;
        _stringToDecimalProvider = stringToDecimalProvider;
    }

    public async Task SetPercentage(string featureName, decimal percentage)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return;
        }

        var feature = await _featureStorage.GetFeature(featureName) ?? new Feature(featureName);
        feature.Percentage = Math.Max(Math.Min(percentage, 100), 0);
        await _featureStorage.StoreFeature(feature);
    }

    public async Task SetGroups(string featureName, IList<string> groups)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return;
        }

        if (!groups.Any())
        {
            return;
        }

        var feature = await _featureStorage.GetFeature(featureName) ?? new Feature(featureName);
        feature.Groups = feature.Groups.Concat(groups).Distinct().ToList();
        await _featureStorage.StoreFeature(feature);
    }

    public async Task RemoveGroups(string featureName, IList<string> groups)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return;
        }

        if (!groups.Any())
        {
            return;
        }

        var feature = await _featureStorage.GetFeature(featureName);
        if (feature is null)
        {
            return;
        }

        feature.Groups = feature.Groups.Except(groups).ToList();

        await _featureStorage.StoreFeature(feature);
    }

    public async Task SetUsers(string featureName, IList<string> users)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return;
        }

        if (!users.Any())
        {
            return;
        }

        var feature = await _featureStorage.GetFeature(featureName) ?? new Feature(featureName);
        feature.Users = feature.Users.Concat(users).Distinct().ToList();
        await _featureStorage.StoreFeature(feature);
    }

    public async Task RemoveUsers(string featureName, IList<string> users)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return;
        }

        if (!users.Any())
        {
            return;
        }

        var feature = await _featureStorage.GetFeature(featureName);
        if (feature is null)
        {
            return;
        }

        feature.Users = feature.Users.Except(users).ToList();

        await _featureStorage.StoreFeature(feature);
    }

    public async Task<bool> IsActiveFor(string featureName, string? user = null, string? group = null)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return false;
        }

        var feature = await _featureStorage.GetFeature(featureName);
        if (feature is null)
        {
            return false;
        }

        if (IsActiveForEveryOne(feature))
        {
            return true;
        }

        return IsActiveForGroup(feature, group) || IsActiveForUser(feature, user);
    }

    private bool IsActiveForGroup(Feature feature, string? group)
    {
        return !string.IsNullOrEmpty(group) && feature.Groups.Contains(group);
    }

    private bool IsActiveForUser(Feature feature, string? user)
    {
        return !string.IsNullOrEmpty(user) &&
               (feature.Users.Contains(user) || _stringToDecimalProvider.Transform(user) < feature.Percentage);
    }

    private bool IsActiveForEveryOne(Feature feature)
    {
        return feature.Percentage == 100 || feature.Groups.Contains("all");
    }

    public async Task Deactivate(string featureName)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return;
        }

        var feature = await _featureStorage.GetFeature(featureName);
        if (feature is null)
        {
            return;
        }

        await _featureStorage.StoreFeature(new Feature(featureName) { Description = feature.Description });
    }

    public Task<Feature?> GetFeature(string featureName)
    {
        return _featureStorage.GetFeature(featureName);
    }

    public async Task SetFeature(Feature feature)
    {
        await _featureStorage.StoreFeature(feature);
    }

    public Task<IList<Feature>> GetAllFeatures()
    {
        return _featureStorage.GetAllFeatures();
    }
}
