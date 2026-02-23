using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithPrivateAndProtectedAttribute
    {
        [Resolve] private readonly SimpleService _simple;
        [Resolve] protected readonly AnotherService Another;

        public SimpleService Simple => _simple;
        public AnotherService AnotherService => Another;
    }
}
