namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithIdMethodParameters
    {
        public SimpleService Simple;
        public AnotherService Another;

        [Attributes.Resolve]
        public void Resolve(
            [Attributes.Resolve(id: "simple")] SimpleService simple,
            AnotherService another
        )
        {
            Simple = simple;
            Another = another;
        }
    }
}