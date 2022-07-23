using FluentAssertions;
using Rollout.Lib.Implementations;

namespace Rollout.Lib.UnitTests;

public class FeatureManagerTests : TestBase
{
    private readonly FeatureManager _featureManager;

    public FeatureManagerTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage);
    }

    [Fact]
    public async Task SetPercentage_PersistCorrectPercentageForFeature()
    {
        // Arrange
        const string featureName = "test";
        const decimal percentage = 50;

        // Act
        await _featureManager.SetPercentage(featureName, percentage);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Percentage.Should().Be(percentage);
    }

    [Fact]
    public async Task SetPercentage_WhenFeatureNameIsBlank_DoesNotAlterFeaure()
    {
        // Arrange
        const string featureName = "";
        const decimal percentage = 50;

        // Act
        await _featureManager.SetPercentage(featureName, percentage);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().BeNull();
    }
}
