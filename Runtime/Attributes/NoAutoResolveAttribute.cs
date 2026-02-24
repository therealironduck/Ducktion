using System;

namespace TheRealIronDuck.Ducktion.Attributes
{
    /// <summary>
    /// This attribute will prevent any service from being automatically resolved.
    /// Meaning the service needs to be registered explicitly. This can be helpful if the service
    /// has parameters that cannot be resolved smartly.
    ///
    /// As an example within the core, see the "TaggedServices" class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NoAutoResolveAttribute : Attribute
    {
    }
}
