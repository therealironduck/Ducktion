using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithResolveMethodAndResolveTags
    {
        public TaggedServices Simple { get; private set; }

        [Resolve]
        public void ThisIsAMethodForSure([ResolveTags(tag: "example")] TaggedServices simple)
        {
            Simple = simple;
        }
    }
}
