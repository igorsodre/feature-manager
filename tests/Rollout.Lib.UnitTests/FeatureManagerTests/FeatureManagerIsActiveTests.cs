using FluentAssertions;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerIsActiveTests : TestBase
{
    private readonly IFeatureManager _featureManager;

    public FeatureManagerIsActiveTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage);
    }

    [Fact]
    public async Task IsActiveFor_WhenPercentageIs100_ReturnsTrue()
    {
        // Arrange
        const string featurName = "test";
        await _featureManager.SetPercentage(featurName, 100);

        // Act
        var result = await _featureManager.IsActiveFor(featurName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsActiveFor_WhenGroupAllIsSet_ReturnsTrue()
    {
        // Arrange
        const string featurName = "test";
        await _featureManager.SetGroups(featurName, new[] { "all" });

        // Act
        var result = await _featureManager.IsActiveFor(featurName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsActiveFor_WhenGroupIsSet_ReturnsTrue()
    {
        // Arrange
        const string featurName = "test";
        await _featureManager.SetGroups(featurName, new[] { "group1", "group2" });

        // Act
        var result = await _featureManager.IsActiveFor(featurName, group: "group1");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsActiveFor_WhenUserIsSet_ReturnsTrue()
    {
        // Arrange
        const string featurName = "test";
        await _featureManager.SetUsers(featurName, new[] { "user1", "user2" });

        // Act
        var result = await _featureManager.IsActiveFor(featurName, user: "user1");

        // Assert
        result.Should().BeTrue();
    }
}
