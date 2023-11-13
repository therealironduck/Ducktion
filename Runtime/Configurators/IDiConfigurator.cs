namespace TheRealIronDuck.Ducktion.Configurators
{
    /// <summary>
    /// You should extend this interface to register your dependencies.
    /// </summary>
    public interface IDiConfigurator
    {
        /// <summary>
        /// In this method you may use the container to register your dependencies.
        /// Please note that you should not use the container to resolve dependencies at
        /// this stage, as it may not be fully configured yet.
        /// </summary>
        /// <param name="container">The dependency injection container</param>
        public void Register(DiContainer container);
    }
}