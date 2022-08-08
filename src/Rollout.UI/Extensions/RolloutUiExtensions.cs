using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Rollout.Lib.Extensions;

namespace Rollout.UI.Extensions;

public static class RolloutUiExtensions
{
    public static IServiceCollection AddRolloutUi(this IServiceCollection services, IWebHostBuilder builder)
    {
        // builder.UseWebRoot("wwwroot");
        builder.UseStaticWebAssets();
        services.AddRazorPages();
        services.AddRollout();
        return services;
    }

    /// <summary>
    /// Adds the pages for the rollout ui. It Requires UseStaticFiles to have been called earlier in the setup
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder UserRolloutUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRazorPages();
        return endpoints;
    }
}
