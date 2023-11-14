namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithPublicAttribute
    {
        [Attributes.Resolve] public readonly SimpleService Simple;

        public readonly AnotherService Another;
        
        public ServiceWithPublicAttribute(AnotherService another)
        {
            Another = another;
        }
    }
}