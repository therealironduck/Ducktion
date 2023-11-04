using System;

namespace TheRealIronDuck.Ducktion.Exceptions
{
    /// <summary>
    /// This exception is thrown when a service could not be resolved from the container.
    /// </summary>
    public class DependencyResolveException : DucktionException
    {
        public DependencyResolveException(Type service, string reason, Exception inner = null) : base(
            $"The service {service} could not be resolved. Reason: {reason}", inner
        )
        {
        }
    }
}