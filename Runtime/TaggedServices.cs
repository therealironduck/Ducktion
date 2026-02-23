using System.Collections;
using System.Collections.Generic;

namespace TheRealIronDuck.Ducktion
{
    public class TaggedServices : IReadOnlyCollection<ServiceDefinition>
    {
        readonly private List<ServiceDefinition> _services;
        readonly private DiContainer _container;

        public TaggedServices(DiContainer container, List<ServiceDefinition> services)
        {
            _services = services;
            _container = container;
        }

        public IEnumerator<ServiceDefinition> GetEnumerator()
        {
            return ((IEnumerable<ServiceDefinition>)_services).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator GetServices()
        {
            foreach (var definition in _services)
            {
                yield return _container.Resolve(definition.ServiceType, definition.Id);
            }
        }

        public IEnumerator<T> GetServices<T>()
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

        public int Count
        {
            get { return _services.Count; }
        }
    }
}
