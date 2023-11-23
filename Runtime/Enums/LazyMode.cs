namespace TheRealIronDuck.Ducktion.Enums
{
    /// <summary>
    /// This enum is used to define the lazy mode of a service. Lazy means that the service
    /// will be instantiated only when it is requested. NonLazy means that the service will
    /// be instantiated when the container is initialized.
    /// </summary>
    public enum LazyMode
    {
        Lazy,
        NonLazy
    }
}