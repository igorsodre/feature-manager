using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rollout.Lib.Interfaces;

namespace Rollout.UI.Areas.Rollout.Pages;

public class Delete : PageModel
{
    private readonly IFeatureManager _featureManager;

    public Delete(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public async Task OnGet([FromQuery] string? featureName)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            Response.Redirect("Index");
            return;
        }

        await _featureManager.DeleteFeature(featureName);

        Response.Redirect("Index");
    }
}
