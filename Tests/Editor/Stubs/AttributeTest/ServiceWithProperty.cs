namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithProperty
    {
        [Attributes.Resolve] public SimpleService Simple { get; private set; }
    }
}