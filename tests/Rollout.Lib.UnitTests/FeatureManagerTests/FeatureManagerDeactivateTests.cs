using FluentAssertions;
using NSubstitute;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerDeactivateTests : TestBase
{
    private readonly IFeatureManager _featureManager;
    private readonly IStringToDecimalProvider _stringToDecimalProvider = Substitute.For<IStringToDecimalProvider>();

    public FeatureManagerDeactivateTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage, _stringToDecimalProvider);
    }

    [Fact]
    public async Task Deactivate_ResetsAllPropertiesForFeaure()
    {
        // Arrange
        const string featureName = "test";
        await Storage.StoreFeature(
            new Feature(featureName)
            {
                Percentage = 5,
                Users = new[] { "user1", "user2" },
                Groups = new[] { "group1", "group2" },
            }
        );

        // Act
        await _featureManager.Deactivate(featureName);

        // Assert
        var feature = await Storage.GetFeature(featureName);

        feature.Should().NotBeNull();
        feature!.Percentage.Should().Be(0);
        feature.Users.Should().BeEmpty();
        feature.Groups.Should().BeEmpty();
    }
}
