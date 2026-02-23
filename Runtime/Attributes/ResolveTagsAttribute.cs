using System;
using JetBrains.Annotations;

namespace TheRealIronDuck.Ducktion.Attributes
{
    /// <summary>
    /// This attribute can be used in combination with the `TaggedServices` class to defines which tag
    /// should be used to resolve the `TaggedServices` instance. It can be used on public fields, private fields,
    /// constructor parameters and method parameters.
    ///
    /// In Unity every scene object that exists at the start of the scene will automatically resolve all
    /// fields that have this attribute. This is done by the DiContainer itself. There is also the
    /// `DynamicDependencyResolver` component which can be used to resolve game objects that are created
    /// later on in the scene.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class ResolveTagsAttribute : Attribute
    {
        public readonly string Tag;

        public ResolveTagsAttribute(string tag)
        {
            Tag = tag;
        }
    }
}
