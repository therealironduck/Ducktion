using System;
using JetBrains.Annotations;

namespace TheRealIronDuck.Ducktion
{
    public class ServiceDefinition
    {
        public readonly Type ServiceType;
        [CanBeNull] public object Instance;
        [CanBeNull] public Func<object> Callback;

        public ServiceDefinition(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}