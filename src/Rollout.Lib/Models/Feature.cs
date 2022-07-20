namespace Rollout.Lib.Models;

internal class Feature
{
    public string Name { get; set; } = string.Empty;

    public decimal Percentage { get; set; }

    public IList<string> Users { get; set; } = Array.Empty<string>();

    public IList<string> Groups { get; set; } = Array.Empty<string>();

    public string Description { get; set; } = string.Empty;

    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    public Feature() { }

    public Feature(string name)
    {
        Name = name;
    }
}
