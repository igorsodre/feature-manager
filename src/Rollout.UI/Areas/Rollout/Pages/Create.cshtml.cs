using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rollout.Lib.Interfaces;
using Rollout.Lib.Models;
using Rollout.UI.Models;

namespace Rollout.UI.Areas.Rollout.Pages;

public class Create : PageModel
{
    private readonly IFeatureManager _featureManager;

    public Create(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public void OnGet() { }

    public async Task OnPost([FromForm] CreateFeatureDto model)
    {
        if (!ModelState.IsValid)
        {
            Response.Redirect($"Details?featureName={model.Name}");
            return;
        }

        var feature = new Feature(model.Name)
        {
            Users = string.IsNullOrEmpty(model.Users) ? new List<string>() : model.Users.Split(';').ToList(),
            Groups = string.IsNullOrEmpty(model.Groups) ? new List<string>() : model.Groups.Split(';').ToList(),
            Percentage = model.Percentage
        };

        await _featureManager.SetFeature(feature);

        Response.Redirect("Index");
    }
}
