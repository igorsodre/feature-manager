using FluentAssertions;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerSetGroupsTests : TestBase
{
    private readonly IFeatureManager _featureManager;

    public FeatureManagerSetGroupsTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage);
    }

    [Fact]
    public async Task SetGroups_ShouldPersistGroupsForFeature()
    {
        // Arrange
        const string featureName = "test";
        var groups = new List<string>
        {
            "group1",
            "group2"
        };

        // Act
        await _featureManager.SetGroups(featureName, groups);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Groups.Should().BeEquivalentTo(groups);
    }

    [Fact]
    public async Task SetGroups_WhenGroupsIsEmpty_DoesNotAlterFeature()
    {
        // Arrange
        const string featureName = "test";
        var groups = new List<string>();

        // Act
        await _featureManager.SetGroups(featureName, groups);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().BeNull();
    }

    [Fact]
    public async Task SetGroups_DoesNotDuplicateGroups()
    {
        // Arrange
        const string featureName = "test";
        var firstGroup = new List<string>
        {
            "group1",
            "group2"
        };

        var secondGroup = new List<string> { "group1" };

        // Act
        await _featureManager.SetGroups(featureName, firstGroup);
        await _featureManager.SetGroups(featureName, secondGroup);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Groups.Should().BeEquivalentTo(firstGroup);
    }

    [Fact]
    public async Task SetGroups_ConcatenatesPreexistingGroupsWithNewOnes()
    {
        // Arrange
        const string featureName = "test";
        var firstGroup = new List<string>
        {
            "group1",
            "group2"
        };

        var secondGroup = new List<string>
        {
            "group3",
            "group4"
        };

        // Act
        await _featureManager.SetGroups(featureName, firstGroup);
        await _featureManager.SetGroups(featureName, secondGroup);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Groups.Should().BeEquivalentTo(firstGroup.Concat(secondGroup));
    }
}
