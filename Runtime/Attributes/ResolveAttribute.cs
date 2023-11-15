using System;
using JetBrains.Annotations;

namespace TheRealIronDuck.Ducktion.Attributes
{
    /// <summary>
    /// This attribute can be used to define services that should be resolved when resolving the main service.
    /// It can be used on public fields, private fields, constructor parameters, method parameters and even
    /// whole methods.
    ///
    /// In Unity every scene object that exists at the start of the scene will automatically resolve all
    /// fields that have this attribute. This is done by the DiContainer itself. There is also the
    /// `DynamicDependencyResolver` component which can be used to resolve game objects that are created
    /// later on in the scene.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class ResolveAttribute : Attribute
    {
    }
}