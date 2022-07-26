using FluentAssertions;
using NSubstitute;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerUserTests : TestBase
{
    private readonly IFeatureManager _featureManager;
    private readonly IStringToDecimalProvider _stringToDecimalProvider = Substitute.For<IStringToDecimalProvider>();

    public FeatureManagerUserTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage, _stringToDecimalProvider);
    }

    [Fact]
    public async Task SetUsers_ShouldPersistUsersForFeature()
    {
        // Arrange
        const string featureName = "test";
        var users = new List<string>
        {
            "user1",
            "user2"
        };

        // Act
        await _featureManager.SetUsers(featureName, users);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Users.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task SetUsers_WhenUsersIsEmpty_DoesNotAlterFeature()
    {
        // Arrange
        const string featureName = "test";
        var users = new List<string>();

        // Act
        await _featureManager.SetUsers(featureName, users);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().BeNull();
    }

    [Fact]
    public async Task SetUsers_DoesNotDuplicateUsers()
    {
        // Arrange
        const string featureName = "test";
        var firstUsers = new List<string>
        {
            "user1",
            "user2"
        };

        var secondUsers = new List<string> { "user1" };

        // Act
        await _featureManager.SetUsers(featureName, firstUsers);
        await _featureManager.SetUsers(featureName, secondUsers);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Users.Should().BeEquivalentTo(firstUsers);
    }

    [Fact]
    public async Task SetUsers_ConcatenatesPreexistingUsersWithNewOnes()
    {
        // Arrange
        const string featureName = "test";
        var firstUsers = new List<string>
        {
            "user1",
            "user2"
        };

        var secondUsers = new List<string>
        {
            "user3",
            "user4"
        };

        // Act
        await _featureManager.SetUsers(featureName, firstUsers);
        await _featureManager.SetUsers(featureName, secondUsers);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Users.Should().BeEquivalentTo(firstUsers.Concat(secondUsers));
    }

    [Fact]
    public async Task RemoveUsers_RemovesUsersFromFeature()
    {
        // Arrange
        const string featureName = "test";
        var groups = new List<string>
        {
            "user1",
            "user2",
            "user3"
        };

        await _featureManager.SetUsers(featureName, groups);

        // Act
        await _featureManager.RemoveUsers(featureName, new[] { "user2", "user1" });

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Name.Should().Be(featureName);
        feature.Users.Should().BeEquivalentTo("user3");
    }
}
