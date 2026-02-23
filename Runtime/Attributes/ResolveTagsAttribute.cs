using System;
using JetBrains.Annotations;

namespace TheRealIronDuck.Ducktion.Attributes
{
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
