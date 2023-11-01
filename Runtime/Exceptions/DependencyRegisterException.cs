using System;

namespace TheRealIronDuck.Ducktion.Exceptions
{
    /// <summary>
    /// This exception is thrown when a service could not be registered in the container.
    /// </summary>
    public class DependencyRegisterException : Exception
    {
        public DependencyRegisterException(Type type, string reason)
            : base($"Service of type {type} could not be registered. Reason: {reason}")
        {
        }
    }
}