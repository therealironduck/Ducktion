using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithPrivateAndProtectedTagged
    {
        [ResolveTags("example")] private readonly TaggedServices _simple;
        [ResolveTags("example")] protected readonly TaggedServices Another;

        public TaggedServices Simple => _simple;
        public TaggedServices AnotherService => Another;
    }
}
