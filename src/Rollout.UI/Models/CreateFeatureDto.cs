using System.ComponentModel.DataAnnotations;

namespace Rollout.UI.Models;

public class CreateFeatureDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(0, 100)]
    public decimal Percentage { get; set; }
    
    public string? Groups { get; set; } = string.Empty;

    public string? Users { get; set; } = string.Empty;
}
