using System.Diagnostics.CodeAnalysis;

namespace Rollout.Lib.Exceptions;

[ExcludeFromCodeCoverage]
public class NullFeatureName : RolloutBaseException
{
    public NullFeatureName(string message) : base(message) { }

    public NullFeatureName() : base("Feature name cannot be null or empty") { }
}
