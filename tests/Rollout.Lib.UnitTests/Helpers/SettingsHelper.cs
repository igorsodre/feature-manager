using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Rollout.Lib.UnitTests.Helpers;

public static class SettingsHelper
{
    public static string GetRedisConnectionString()
    {
        var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = Path.Combine(buildDir!, "appsettings.json");
        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(filePath)
            .AddEnvironmentVariables()
            .Build()
            .GetConnectionString("Redis");
    }
}
