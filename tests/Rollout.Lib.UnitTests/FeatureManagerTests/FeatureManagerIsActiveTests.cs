using FluentAssertions;
using NSubstitute;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerIsActiveTests : TestBase
{
    private readonly IFeatureManager _featureManager;
    private readonly IStringToDecimalProvider _stringToDecimalProvider = Substitute.For<IStringToDecimalProvider>();

    public FeatureManagerIsActiveTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage, _stringToDecimalProvider);
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
    public async Task IsActiveFor_WhenPercentageIs0_ReturnsFalse()
    {
        // Arrange
        const string featurName = "test";
        await _featureManager.SetPercentage(featurName, 0);

        // Act
        var result = await _featureManager.IsActiveFor(featurName);

        // Assert
        result.Should().BeFalse();
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
    public async Task IsActiveFor_WhenGroupIsNotSet_ReturnsFalse()
    {
        // Arrange
        const string featurName = "test";
        await _featureManager.SetGroups(featurName, new[] { "group1", "group2" });

        // Act
        var result = await _featureManager.IsActiveFor(featurName, group: "group3");

        // Assert
        result.Should().BeFalse();
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

    [Fact]
    public async Task IsActiveFor_WhenUserIsNotSet_ReturnsFalse()
    {
        // Arrange
        _stringToDecimalProvider.Transform(Arg.Any<string>()).Returns(50);
        const string featurName = "test";
        await _featureManager.SetUsers(featurName, new[] { "user1", "user2" });

        // Act
        var result = await _featureManager.IsActiveFor(featurName, user: "user3");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsActiveFor_WhenNumberRepresentationOfUserIsWithinPercentage_ReturnsTrue()
    {
        // Arrange
        const string featurName = "test";
        _stringToDecimalProvider.Transform("user1").Returns(49);
        await _featureManager.SetPercentage(featurName, 50);

        // Act
        var result = await _featureManager.IsActiveFor(featurName, user: "user1");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsActiveFor_WhenNumberRepresentationOfUserIsOutsidePercentage_ReturnsFalse()
    {
        // Arrange
        const string featurName = "test";
        _stringToDecimalProvider.Transform("user1").Returns(51);
        await _featureManager.SetPercentage(featurName, 50);

        // Act
        var result = await _featureManager.IsActiveFor(featurName, user: "user1");

        // Assert
        result.Should().BeFalse();
    }
}
