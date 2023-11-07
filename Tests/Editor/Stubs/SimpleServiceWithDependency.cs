namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs
{
    public class SimpleServiceWithDependency : ISimpleInterface
    {
        public readonly AnotherService Another;
        
        public SimpleServiceWithDependency(AnotherService another)
        {
            Another = another;
        }
    }
}