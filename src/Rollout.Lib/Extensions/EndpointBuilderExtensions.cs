using Rollout.Lib.UI.Endpoints;

namespace Rollout.Lib.Extensions;

public static class EndpointBuilderExtensions
{
    public static IEndpointRouteBuilder MapRollout(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/internal/rollout-ui", Routes.Home);
        builder.MapGet("/internal/rollout-ui/new", Routes.AddFeature);

        return builder;
    }
}
