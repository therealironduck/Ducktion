using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithIdMethodParameters
    {
        public SimpleService Simple;
        public AnotherService Another;

        [Resolve]
        public void Resolve(
            [Resolve(id: "simple")] SimpleService simple,
            AnotherService another
        )
        {
            Simple = simple;
            Another = another;
        }
    }
}
