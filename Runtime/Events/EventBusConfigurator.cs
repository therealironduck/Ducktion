using TheRealIronDuck.Ducktion.Configurators;

namespace TheRealIronDuck.Ducktion.Events
{
    /// <summary>
    /// This configurator registers any service which is required for the eventbus to function.
    /// </summary>
    public class EventBusConfigurator : IDiConfigurator
    {
        /// <summary>
        /// In this method you may use the container to register your dependencies.
        /// Please note that you should not use the container to resolve dependencies at
        /// this stage, as it may not be fully configured yet.
        /// </summary>
        /// <param name="container">The dependency injection container</param>
        public void Register(DiContainer container)
        {
            container.Register<EventBus>().Singleton().Lazy();
        }
    }
}