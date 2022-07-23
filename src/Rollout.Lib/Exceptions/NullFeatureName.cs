namespace Rollout.Lib.Exceptions;

public class NullFeatureName : RolloutBaseException
{
    public NullFeatureName(string message) : base(message) { }

    public NullFeatureName() : base("Feature name cannot be null or empty") { }
}
