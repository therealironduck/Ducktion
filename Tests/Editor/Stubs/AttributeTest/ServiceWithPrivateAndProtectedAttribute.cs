namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithPrivateAndProtectedAttribute
    {
        [Attributes.Resolve] private readonly SimpleService _simple;
        [Attributes.Resolve] protected readonly AnotherService Another;

        public SimpleService Simple => _simple;
        public AnotherService AnotherService => Another;
    }
}