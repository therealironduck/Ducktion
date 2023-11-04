namespace TheRealIronDuck.Ducktion.Tests.Stubs
{
    /// <summary>
    /// This service simply has a direct dependency to another service.
    /// </summary>
    public class ServiceWithDependencies
    {
        public readonly ISimpleInterface Simple;
        
        public ServiceWithDependencies(ISimpleInterface simple)
        {
            Simple = simple;
        }
    }
}