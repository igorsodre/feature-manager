using FluentAssertions;
using NSubstitute;
using Rollout.Lib.Implementations;
using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;

namespace Rollout.Lib.UnitTests.FeatureManagerTests;

public class FeatureManagerGetAllTests : TestBase
{
    private readonly IFeatureManager _featureManager;
    private readonly IStringToDecimalProvider _stringToDecimalProvider = Substitute.For<IStringToDecimalProvider>();

    public FeatureManagerGetAllTests(RedisFixture fixture) : base(fixture)
    {
        _featureManager = new FeatureManager(Storage, _stringToDecimalProvider);
    }

    [Fact]
    public async Task GetAllFeatures_SholdRetrieveAllStoredFeatures()
    {
        // Arrange
        var features = new[]
        {
            new Feature("test1") { Users = new[] { "user1" } },
            new Feature("test2") { Users = new[] { "user2" } },
            new Feature("test3") { Users = new[] { "user3" } }
        };

        await Task.WhenAll(features.Select(f => _featureManager.SetUsers(f.Name, f.Users)));

        // Act
        var result = await _featureManager.GetAllFeatures();

        // Assert
        result.Should().BeEquivalentTo(features, options => options.Excluding(obj => obj.LastModified));
    }
}
