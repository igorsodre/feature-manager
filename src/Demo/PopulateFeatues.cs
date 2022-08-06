using Rollout.Lib.Interfaces;

namespace Demo;

public class PopulateFeatues : IHostedService
{
    private readonly IFeatureManager _featureManager;

    public PopulateFeatues(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (await _featureManager.IsActiveFor("feature1.2"))
        {
            return;
        }

        await _featureManager.SetPercentage("feature1", 50);
        await _featureManager.SetPercentage("feature1.2", 100);

        await _featureManager.SetGroups("feature2", new[] { "group1", "group2" });
        await _featureManager.SetGroups("feature2.2", new[] { "group3", "group4" });

        await _featureManager.SetUsers("feature3", new[] { "user1", "user2" });
        await _featureManager.SetUsers("feature3.2", new[] { "user3", "user4" });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
