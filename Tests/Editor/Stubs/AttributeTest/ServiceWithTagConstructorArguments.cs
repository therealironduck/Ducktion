using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithTagConstructorArguments
    {
        public readonly TaggedServices Simple;

        public ServiceWithTagConstructorArguments(
            [ResolveTags(tag: "example")] TaggedServices simple
        )
        {
            Simple = simple;
        }
    }
}
