using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Rollout.Lib.Extensions;
using Rollout.Lib.Interfaces;

namespace Rollout.UI.Extensions;

public static class RolloutUiExtensions
{
    /// <summary>
    /// Adds the UI to manage the feature flags of the application.
    /// It also adds "UseStaticWebAssets" and razor pages support
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="builder">IWebHostBuilder</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddRolloutUi(this IServiceCollection services, IWebHostBuilder builder)
    {
        builder.UseStaticWebAssets();
        services.AddRazorPages();
        services.AddRollout();
        return services;
    }

    /// <summary>
    /// Adds the UI to manage the feature flags of the application.
    /// It also adds "UseStaticWebAssets" and razor pages support
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <typeparam name="TStorage"> implementation of the IFeatureStorage interface </typeparam>
    /// <returns></returns>
    public static IServiceCollection AddRolloutUiWithCustomStorage<TStorage>(
        this IServiceCollection services,
        IWebHostBuilder builder
    ) where TStorage : class, IFeatureStorage
    {
        builder.UseStaticWebAssets();
        services.AddRazorPages();
        services.AddRolloutWithCustomStorage<TStorage>();
        return services;
    }

    /// <summary>
    /// Adds the UI to manage the feature flags of the application.
    /// It also adds "UseStaticWebAssets" and razor pages support
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <typeparam name="TStorage"> implementation of the IFeatureStorage interface </typeparam>
    /// <typeparam name="TStringToDecimalProvider"> implementation of the IStringToDecimalProvider interface </typeparam>
    /// <returns></returns>
    public static IServiceCollection
        AddRolloutUiWithCustomStorageAndStringToDecimalProvider<TStorage, TStringToDecimalProvider>(
            this IServiceCollection services,
            IWebHostBuilder builder
        ) where TStorage : class, IFeatureStorage where TStringToDecimalProvider : class, IStringToDecimalProvider
    {
        builder.UseStaticWebAssets();
        services.AddRazorPages();
        services.AddRolloutWithCustomStorageAndStringToDecimalProvider<TStorage, TStringToDecimalProvider>();
        return services;
    }

    /// <summary>
    /// register the pages for the rollout ui. It Requires "UseStaticFiles" to have been called earlier in the setup
    /// </summary>
    /// <param name="endpoints"> IEndpointRouteBuilder </param>
    /// <returns> IEndpointRouteBuilder </returns>
    public static IEndpointRouteBuilder MapRolloutUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRazorPages();
        return endpoints;
    }
}
