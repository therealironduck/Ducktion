using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithIdConstructorArguments
    {
        public readonly SimpleService Simple;
        public readonly AnotherService Another;

        public ServiceWithIdConstructorArguments(
            [Resolve(id: "simple")] SimpleService simple,
            AnotherService another
        )
        {
            Simple = simple;
            Another = another;
        }
    }
}
