using Microsoft.AspNetCore.Mvc;
using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;
using Rollout.Lib.UI.Common;

namespace Rollout.Lib.UI.Endpoints;

public static class Routes
{
    public static async Task Home(
        HttpContext context,
        [FromServices] ViewRender renderer,
        IFeatureManager featureManager
    )
    {
        var features = await featureManager.GetAllFeatures();
        var content = await renderer.Render("~/UI/Pages/Index.cshtml", features);

        var response = context.Response;
        response.ContentType = "text/html";
        await response.WriteAsync(content);
    }

    public static async Task AddFeature(
        HttpContext context,
        [FromQuery] string featureName,
        [FromServices] ViewRender renderer,
        IFeatureManager featureManager
    )
    {
        var feature = (await featureManager.GetFeature(featureName)) ?? new Feature();
        var content = await renderer.Render("~/UI/Pages/AddFeature.cshtml", feature);

        var response = context.Response;
        response.ContentType = "text/html";
        await response.WriteAsync(content);
    }
}
