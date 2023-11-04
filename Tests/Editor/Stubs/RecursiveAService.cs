namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs
{
    /// <summary>
    /// This is a service which requires another service in its constructor.
    /// The other service also requires this service in its constructor.
    /// So there is a circular dependency.
    /// </summary>
    public class RecursiveAService
    {
        public RecursiveAService(RecursiveBService b)
        {
        }
    }
}