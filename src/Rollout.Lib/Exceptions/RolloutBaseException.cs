namespace Rollout.Lib.Exceptions;

public abstract class RolloutBaseException : Exception
{
    protected RolloutBaseException(string message) : base(message) { }
}
