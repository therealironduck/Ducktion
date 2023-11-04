﻿using System;
using System.Collections.Generic;
using System.Linq;
using TheRealIronDuck.Ducktion.Exceptions;
using UnityEngine;

namespace TheRealIronDuck.Ducktion
{
    /// <summary>
    /// This is the core component of this whole package. It holds a list of all registered services
    /// and their concrete implementations. It also stores all resolved instances as singletons.
    ///
    /// There can only ever be one container active at a time and it can be configured through code
    /// or the editor.
    ///
    /// If you want to start quickly, simply attach the component to any game object or call the
    /// `Ducktion.singleton` from anywhere in your code.
    /// </summary>
    public class DiContainer : MonoBehaviour
    {
        #region EXPOSED FIELDS

        /// <summary>
        /// When this is toggled on, unity wont destroy this object when changing scenes. If you want to have
        /// a separate container for each scene, you should disable this. 
        /// </summary>
        [Header("Options")] [SerializeField] private bool dontDestroyOnLoad = true;

        #endregion

        #region VARIABLES

        /// <summary>
        /// This variable contains all registered service references. The key is for example the interface
        /// with the value being the concrete implementation type. In a lot of cases both key and value
        /// can be the same type.
        /// </summary>
        private readonly Dictionary<Type, Type> _services = new();

        /// <summary>
        /// This dictionary stores every resolved instance so that it can be returned as a singleton.
        /// </summary>
        private readonly Dictionary<Type, object> _instances = new();

        #endregion

        #region LIFECYCLE METHODS

        /// <summary>
        /// Handle some initialization logic. If the `dontDestroyOnLoad` flag is set, the container
        /// wont be destroyed when changing scenes.
        ///
        /// Also this method registers the container in the static `Ducktion` class, so that it can
        /// be called from anywhere in the code using `Ducktion.singleton`.
        /// </summary>
        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            Ducktion.RegisterContainer(this);
        }

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
            var keyType = typeof(TKey);
            var serviceType = typeof(TService);

            ValidateService(keyType, serviceType);
            
            if (_services.ContainsKey(typeof(TKey)))
            {
                throw new DependencyRegisterException(
                    keyType,
                    "Service is already registered. Use `override` to override the service"
                );
            }

            _services.Add(keyType, serviceType);
        }

        /// <summary>
        /// Override any registered service with another implementation. Any singleton instance for this type
        /// will be cleared as well.
        ///
        /// The service itself must not be abstract or an enum.
        /// </summary>
        /// <typeparam name="TKey">The type which gets registered</typeparam>
        /// <typeparam name="TService">The concrete implementation type</typeparam>
        /// <exception cref="DependencyRegisterException">If the override fails, it will throw an error</exception>
        public void Override<TKey, TService>() where TService : TKey
        {
            var keyType = typeof(TKey);
            var serviceType = typeof(TService);
            
            ValidateService(keyType, serviceType);

            if (!_services.ContainsKey(keyType))
            {
                throw new DependencyRegisterException(
                    keyType,
                    "Service is not registered. Use `register` to register the service"
                );
            }
            
            _services[keyType] = serviceType;
            _instances.Remove(keyType);
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
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve a given service from the container. It will instantiate the concrete implementation
        /// and return it.
        ///
        /// By default all returned services are stored as singleton. So if you request the same service
        /// twice, you will get the same instance. 
        /// </summary>
        /// <param name="type">The type which should be resolved</param>
        /// <returns>The singleton instance</returns>
        /// <exception cref="DependencyResolveException">If the type couldn't be resolved, an error will be thrown</exception>
        public object Resolve(Type type)
        {
            return InnerResolve(type, new[] { type });
        }

        /// <summary>
        /// Remove all registered services and singleton instances, basically resetting the container.
        /// </summary>
        public void Clear()
        {
            _services.Clear();
            _instances.Clear();
        }

        /// <summary>
        /// Reset every singleton instance. This will not remove the registered services.
        /// If you want to reset everything, use `Clear` instead.
        /// </summary>
        public void Reset()
        {
            _instances.Clear();
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Inner logic to resolve a component. This method handles the recursive resolving of all
        /// parameters of the constructor. Also it checks for circular dependencies.
        /// </summary>
        /// <param name="type">The type which should be resolved</param>
        /// <param name="dependencyChain">Internal variable to keep track of the whole chain of dependencies</param>
        /// <returns>The singleton instance</returns>
        /// <exception cref="DependencyResolveException">If the type couldn't be resolved, an error will be thrown</exception>
        private object InnerResolve(Type type, Type[] dependencyChain)
        {
            if (!_services.ContainsKey(type))
            {
                throw new DependencyResolveException(type, "Service is not registered");
            }

            if (_instances.TryGetValue(type, out var singleton))
            {
                return singleton;
            }

            var constructors = type.GetConstructors();
            if (constructors.Length > 1)
            {
                throw new DependencyResolveException(type, "Service has more than one constructor");
            }

            var parameters = new List<object>();

            foreach (var constructorInfo in constructors)
            {
                foreach (var parameter in constructorInfo.GetParameters())
                {
                    if (dependencyChain.Contains(parameter.ParameterType))
                    {
                        throw new DependencyResolveException(
                            type,
                            $"Circular dependency detected for parameter `{parameter.Name}`"
                        );
                    }

                    try
                    {
                        var newChain = dependencyChain.Append(parameter.ParameterType).ToArray();

                        parameters.Add(InnerResolve(parameter.ParameterType, newChain));
                    }
                    catch (DependencyResolveException exception)
                    {
                        throw new DependencyResolveException(
                            type,
                            $"Parameter `{parameter.Name}` could not be resolved",
                            exception
                        );
                    }
                }
            }

            var instance = Activator.CreateInstance(_services[type], parameters.ToArray());
            _instances.Add(type, instance);

            return instance;
        }

        private static void ValidateService(Type keyType, Type serviceType)
        {
            if (serviceType.IsAbstract)
            {
                throw new DependencyRegisterException(keyType, "Service is abstract");
            }

            if (serviceType.IsEnum)
            {
                throw new DependencyRegisterException(keyType, "Service is an enum");
            }
        }
        
        #endregion
    }
}