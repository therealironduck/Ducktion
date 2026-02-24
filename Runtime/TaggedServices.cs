using System.Collections;
using System.Collections.Generic;
using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion
{
    /// <summary>
    /// This collection hold all service definitions for any given tag. It should only ever be used
    /// in combination with the `DiContainer::GetTagged` method or `[ResolveTags]` attribute.
    ///
    /// The collection itself holds the ServiceDefinitions. You can use the `GetServices` method to get
    /// an iterator for all resolved services. Before using that method, services are not automatically resolved.
    /// </summary>
    [NoAutoResolve]
    public class TaggedServices : IReadOnlyCollection<ServiceDefinition>
    {
        /// <summary>
        /// All service definitions which match the tag
        /// </summary>
        readonly private List<ServiceDefinition> _services;

        /// <summary>
        /// A reference to the DIContainer which instantiated this service
        /// </summary>
        readonly private DiContainer _container;

        /// <summary>
        /// Internal constructor to create an instance. It gets the services and a reference to the container.
        /// </summary>
        /// <param name="container">Reference to the DIContainer</param>
        /// <param name="services">All service definitions which match the tag</param>
        internal TaggedServices(DiContainer container, List<ServiceDefinition> services)
        {
            _services = services;
            _container = container;
        }

        /// <summary>
        /// Allows to iterate through all service definitions.
        /// </summary>
        /// <returns>Enumerator which contains all service definitions</returns>
        public IEnumerator<ServiceDefinition> GetEnumerator()
        {
            return ((IEnumerable<ServiceDefinition>)_services).GetEnumerator();
        }

        /// <summary>
        /// Alias for the other `GetEnumerator` without a specified type.
        /// </summary>
        /// <returns>Enumerator which contains all service definitions</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Allows to iterate through all resolved services. They will only be resolved when
        /// being accessed.
        /// </summary>
        /// <returns>Enumerable which contains all resolved services</returns>
        public IEnumerable GetServices()
        {
            foreach (var definition in _services)
            {
                yield return _container.Resolve(definition.ServiceType, definition.Id);
            }
        }

        /// <summary>
        /// This method allows to filter by a specific type before returning the enumerator.
        /// This can useful if you tag a bunch of classes which inherit the same baseclass and
        /// you want to use static types for it.
        ///
        /// Each service that doesn't extend / is assignable from the given type will be ignored.
        /// </summary>
        /// <typeparam name="T">The basetype which should be filtered</typeparam>
        /// <returns>A filtered enumerable for the resolved services that match the type</returns>
        public IEnumerable<T> GetServices<T>()
        {
            foreach (var definition in _services)
            {
                if (!typeof(T).IsAssignableFrom(definition.ServiceType))
                {
                    continue;
                }

                yield return (T)_container.Resolve(definition.ServiceType, definition.Id);
            }
        }

        /// <summary>
        /// The amount of services with the given tag.
        /// </summary>
        public int Count
        {
            get { return _services.Count; }
        }
    }
}
