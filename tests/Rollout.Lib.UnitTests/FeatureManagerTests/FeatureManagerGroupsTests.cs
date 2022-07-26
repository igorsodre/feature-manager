using FluentAssertions;
using NSubstitute;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerSetGroupsTests : TestBase
{
    private readonly IFeatureManager _featureManager;
    private readonly IStringToDecimalProvider _stringToDecimalProvider = Substitute.For<IStringToDecimalProvider>();
    public FeatureManagerSetGroupsTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage, _stringToDecimalProvider);
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

    [Fact]
    public async Task RemoveGroups_RemovesGroupsFromFeature()
    {
        // Arrange
        const string featureName = "test";
        var groups = new List<string>
        {
            "group1",
            "group2",
            "group3"
        };

        await _featureManager.SetGroups(featureName, groups);

        // Act
        await _featureManager.RemoveGroups(featureName, new[] { "group2", "group1" });

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Groups.Should().BeEquivalentTo("group3");
    }
}
