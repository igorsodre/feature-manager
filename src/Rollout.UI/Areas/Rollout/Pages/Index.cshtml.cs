using Microsoft.AspNetCore.Mvc.RazorPages;
using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;

namespace Rollout.UI.Areas.Rollout.Pages;

public class IndexModel : PageModel
{
    private readonly IFeatureManager _featureManager;

    public IList<Feature> Features { get; private set; } = new List<Feature>();

    public IndexModel(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public async Task OnGet()
    {
        Features = await _featureManager.GetAllFeatures();
    }
}
