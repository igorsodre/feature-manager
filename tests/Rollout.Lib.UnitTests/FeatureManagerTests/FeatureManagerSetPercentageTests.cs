using FluentAssertions;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerSetPercentageTests : TestBase
{
    private readonly IFeatureManager _featureManager;

    public FeatureManagerSetPercentageTests(RedisFixture fixture) : base(fixture)
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
    public async Task SetPercentage_WhenFeatureNameIsBlank_DoesNotAlterFeature()
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

    [Fact]
    public async Task SetPercentage_WhenPercentageIsLessThanZero_SetItToZero()
    {
        // Arrange
        const string featureName = "test";
        const decimal percentage = -50;

        // Act
        await _featureManager.SetPercentage(featureName, percentage);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Percentage.Should().Be(0);
    }

    [Fact]
    public async Task SetPercentage_WhenPercentageIsGreaterThan100_SetItTo100()
    {
        // Arrange
        const string featureName = "test";
        const decimal percentage = 150;

        // Act
        await _featureManager.SetPercentage(featureName, percentage);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Percentage.Should().Be(100);
    }
}
