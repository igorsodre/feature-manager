using System.Diagnostics.CodeAnalysis;

namespace Rollout.Lib.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class RolloutBaseException : Exception
{
    protected RolloutBaseException(string message) : base(message) { }
}
