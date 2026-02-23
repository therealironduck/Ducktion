using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithPropertyTagged
    {
        [ResolveTags("example")] public TaggedServices Simple { get; private set; }
    }
}
