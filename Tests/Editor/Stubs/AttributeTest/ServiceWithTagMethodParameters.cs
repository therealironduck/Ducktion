using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithTagMethodParameters
    {
        public TaggedServices Simple;

        [Resolve]
        public void Resolve(
            [ResolveTags(tag: "example")] TaggedServices simple
        )
        {
            Simple = simple;
        }
    }
}
