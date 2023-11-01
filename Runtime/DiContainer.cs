using System;
using System.Collections.Generic;
using TheRealIronDuck.Ducktion.Exceptions;
using UnityEngine;

namespace TheRealIronDuck.Ducktion
{
    public class DiContainer : MonoBehaviour
    {
        #region VARIABLES

        /// <summary>
        /// This variable contains all registered service references. The key is for example the interface
        /// with the value being the concrete implementation type. In a lot of cases both key and value
        /// can be the same type.
        /// </summary>
        private Dictionary<Type, Type> _services = new();

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Register a new service. The service type is used as the key and the concrete implementation.
        /// The service must not be abstract or an enum.
        /// </summary>
        /// <typeparam name="T">The type which should be registered</typeparam>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public void Register<T>() => Register<T, T>();

        /// <summary>
        /// Register a new service for a given type. The service must be the same as the type, or a child of it.
        /// For for type it could be an interface with the service being the concrete implementation.
        ///
        /// The service itself must not be abstract or an enum.
        /// </summary>
        /// <typeparam name="TKey">The type which gets registered</typeparam>
        /// <typeparam name="TService">The concrete implementation type</typeparam>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public void Register<TKey, TService>() where TService : TKey
        {
            var serviceType = typeof(TService);

            if (serviceType.IsAbstract)
            {
                throw new DependencyRegisterException(serviceType, "Service is abstract");
            }

            if (serviceType.IsEnum)
            {
                throw new DependencyRegisterException(serviceType, "Service is an enum");
            }

            _services.Add(typeof(TKey), typeof(TService));
        }

        /// <summary>
        /// Resolve a given service from the container. It will instantiate the concrete implementation
        /// and return it.
        ///
        /// By default all returned services are stored as singleton. So if you request the same service
        /// twice, you will get the same instance.
        /// </summary>
        /// <typeparam name="T">The type which should be resolved</typeparam>
        /// <returns>The singleton instance</returns>
        /// <exception cref="DependencyResolveException">If the type couldn't be resolved, an error will be thrown</exception>
        public T Resolve<T>()
        {
            if (!_services.ContainsKey(typeof(T)))
            {
                throw new DependencyResolveException(typeof(T));
            }

            return (T)Activator.CreateInstance(_services[typeof(T)]);
        }

        #endregion
    }
}