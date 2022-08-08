using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rollout.Lib.Interfaces;
using Rollout.UI.Models;

namespace Rollout.UI.Areas.Rollout.Pages;

public class Create : PageModel
{
    private readonly IFeatureManager _featureManager;

    public Create(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public async Task OnGet([FromForm] CreateFeatureDto model)
    {
        if (!ModelState.IsValid)
        {
            Redirect($"Rollout/Details?featureName={model.Name}");
            return;
        }

        // await _featureManager.(model.Name);

        Redirect("Rollout/Index");
    }
}
