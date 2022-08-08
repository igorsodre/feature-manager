using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;

namespace Rollout.UI.Areas.Rollout.Pages;

public class Details : PageModel
{
    public Details(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public Feature Feature { get; private set; } = new();

    private readonly IFeatureManager _featureManager;

    public async Task OnGet([FromQuery] string? featureName)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            return;
        }

        Feature = (await _featureManager.GetFeature(featureName)) ?? new Feature();
    }
}
