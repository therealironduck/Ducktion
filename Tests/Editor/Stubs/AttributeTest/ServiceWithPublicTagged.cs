using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithPublicTagged
    {
        [ResolveTags("example")] public readonly TaggedServices Services;

        public readonly AnotherService Another;

        public ServiceWithPublicTagged(AnotherService another)
        {
            Another = another;
        }
    }
}
