using System;
using System.Collections.Generic;
using System.Linq;
using TheRealIronDuck.Ducktion.Configurators;
using TheRealIronDuck.Ducktion.Enums;
using TheRealIronDuck.Ducktion.Exceptions;
using TheRealIronDuck.Ducktion.Logging;
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
        [SerializeField] private bool dontDestroyOnLoad = true;

        /// <summary>
        /// This is the log level for the container itself. It will be used to log all registered services
        /// and any other actions the container does.
        ///
        /// In production you should set this to `Error` to only log errors. In development you can set it
        /// to `Info` or `Debug` to get more detailed information.
        /// </summary>
        [SerializeField] private LogLevel logLevel = LogLevel.Error;

        /// <summary>
        /// If set, Ducktion will try to automatically resolve any given type. This means you don't need
        /// to register any services manually. Manually registered services however will always take
        /// precedence over automatically resolved ones.
        /// </summary>
        [SerializeField] private bool enableAutoResolve = true;

        /// <summary>
        /// Specify the singleton mode for automatically resolved services. This will only be used if
        /// enableAutoResolve is set to true.
        /// </summary>
        [SerializeField] private SingletonMode autoResolveSingletonMode = SingletonMode.Singleton;

        /// <summary>
        /// Specify the default lazy mode any service should be registered with. This will only be used
        /// if no other lazy mode is specified during registration.
        /// </summary>
        [SerializeField] private LazyMode defaultLazyMode = LazyMode.Lazy;

        /// <summary>
        /// All the default Mono configurators which will be loaded when the container is initialized.
        /// In addition to these, you can also add your own configurators using the `AddConfigurator`
        /// method.
        /// </summary>
        [SerializeField] private MonoDiConfigurator[] defaultConfigurators = Array.Empty<MonoDiConfigurator>();

        #endregion

        #region VARIABLES

        /// <summary>
        /// This variable contains all registered service references. The key is for example the interface
        /// with the value being the service definition. The service definition holds the real type, - in a
        /// lot of cases both key and value can be the same type. It also contains the singleton instances
        /// and other relevant data to resolve the service.
        ///
        /// By default we register our own logger, so that we can log all events happening.
        /// </summary>
        private readonly Dictionary<Type, ServiceDefinition> _services = new()
        {
            { typeof(DucktionLogger), new ServiceDefinition(typeof(DucktionLogger)) }
        };

        /// <summary>
        /// A reference to the logger instance. This is used to log all events happening in the container.
        /// This variable is resolved within the `Reinitialize` method and comes directly from the container.
        /// #EatYourOwnDogFood
        /// </summary>
        private DucktionLogger _logger;

        /// <summary>
        /// This is a list of all registered configurators. It merges the default mono configurators
        /// and manually registered containers together. This list is used in the `Reinitialize` method
        /// and all configurators are called to register their services.
        /// </summary>
        private readonly List<IDiConfigurator> _configurators = new();

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

            _configurators.AddRange(defaultConfigurators);

            Reinitialize();
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Reinitialize the container. This will register the container in the static `Ducktion` class
        /// and create a new logger instance with the configured log level.
        /// </summary>
        public void Reinitialize()
        {
            Ducktion.RegisterContainer(this);

            _logger = Resolve<DucktionLogger>();
            _logger.Configure(logLevel);

            _configurators.ForEach(configurator =>
            {
                _logger.Log(LogLevel.Info, $"Using configurator: {configurator.GetType()}");
                configurator.Register(this);
            });

            InitializeNonLazyServices();

            _logger.Log(LogLevel.Info, "Reinitialized container");
        }

        /// <summary>
        /// This method can be used to configure the container code-wise. It will reinitialize the container
        /// afterwards.
        /// </summary>
        /// <param name="newLevel">The log level</param>
        /// <param name="newEnableAutoResolve">Should auto resolve be enabled?</param>
        /// <param name="newAutoResolveSingletonMode">The singleton mode of auto-resolved services</param>
        /// <param name="newDefaultLazyMode">The default lazy mode</param>
        public void Configure(
            LogLevel newLevel = LogLevel.Error,
            bool newEnableAutoResolve = true,
            SingletonMode newAutoResolveSingletonMode = SingletonMode.Singleton,
            LazyMode newDefaultLazyMode = LazyMode.Lazy
        )
        {
            logLevel = newLevel;
            enableAutoResolve = newEnableAutoResolve;
            autoResolveSingletonMode = newAutoResolveSingletonMode;
            defaultLazyMode = newDefaultLazyMode;

            Reinitialize();
        }

        /// <summary>
        /// Register a new service for a given type. The service must be the same as the type, or a child of it.
        /// For for type it could be an interface with the service being the concrete implementation.
        ///
        /// The service itself must not be abstract or an enum.
        /// </summary>
        /// <param name="keyType">The type which gets registered</param>
        /// <param name="serviceType">The concrete implementation type</param>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register(Type keyType, Type serviceType)
        {
            if (!keyType.IsAssignableFrom(serviceType) && serviceType != typeof(object))
            {
                _logger.Log(LogLevel.Error, $"Service {serviceType} does not extend {keyType}");

                throw new DependencyRegisterException(
                    keyType,
                    $"Service {serviceType} does not extend {keyType}"
                );
            }

            ValidateService(keyType, serviceType);

            if (_services.ContainsKey(keyType))
            {
                _logger.Log(LogLevel.Error, $"Service {keyType} is already registered");

                throw new DependencyRegisterException(
                    keyType,
                    "Service is already registered. Use `override` to override the service"
                );
            }

            var serviceDefinition = new ServiceDefinition(serviceType);
            _services.Add(keyType, serviceDefinition);
            _logger.Log(LogLevel.Debug, $"Registered service: {keyType} => {serviceType}");

            return serviceDefinition;
        }

        /// <summary>
        /// Register a new service. The service type is used as the key and the concrete implementation.
        /// The service must not be abstract or an enum.
        /// </summary>
        /// <param name="type">The type which should be registered</param>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register(Type type) => Register(type, type);

        /// <summary>
        /// Register a new service. The service type is used as the key and the concrete implementation.
        /// The service must not be abstract or an enum.
        /// </summary>
        /// <typeparam name="T">The type which should be registered</typeparam>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register<T>() => Register(typeof(T), typeof(T));

        /// <summary>
        /// Register a new service for a given type. The service must be the same as the type, or a child of it.
        /// For for type it could be an interface with the service being the concrete implementation.
        ///
        /// The service itself must not be abstract or an enum.
        /// </summary>
        /// <typeparam name="TKey">The type which gets registered</typeparam>
        /// <typeparam name="TService">The concrete implementation type</typeparam>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register<TKey, TService>() where TService : TKey => Register(typeof(TKey), typeof(TService));

        /// <summary>
        /// Register a new service and its instance. The service type is used as the key and the concrete implementation.
        /// The instance will be registered as a singleton.
        /// </summary>
        /// <param name="instance">The instance which should be returned</param>
        /// <typeparam name="T">The type which should be registered</typeparam>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register<T>(T instance) => Register(typeof(T), instance);

        /// <summary>
        /// Register a new service and its instance. The service type is used as the key and the concrete implementation.
        /// The instance will be registered as a singleton.
        /// </summary>
        /// <param name="type">The type which should be registered</param>
        /// <param name="instance">The instance which should be returned</param>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register(Type type, object instance)
        {
            var definition = Register(type, instance.GetType());
            definition.Instance = instance;

            return definition;
        }

        /// <summary>
        /// Register a callback which gets called on resolve. This is useful if you want to resolve
        /// a service which requires some parameters. The callback will be called on resolve and
        /// be stored as a singleton.
        /// more information.
        /// </summary>
        /// <param name="type">The type which should be registered</param>
        /// <param name="callback">The callback which gets called on resolve. Must return an instance</param>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register<T>(Type type, Func<T> callback)
        {
            var serviceType = callback.Method.ReturnType.IsAbstract ? typeof(object) : type;
            var definition = Register(type, serviceType);
            definition.Callback = () => callback();

            return definition;
        }

        /// <summary>
        /// Register a callback which gets called on resolve. This is useful if you want to resolve
        /// a service which requires some parameters. The callback will be called on resolve and
        /// be stored as a singleton.
        /// more information.
        ///
        /// Since the definition requires a Func(object), we can't simply pass the callback
        /// directly. Instead we wrap the callback into another callback. Quite hacky, but
        /// it is what it is.
        /// </summary>
        /// <typeparam name="T">The type which should be registered</typeparam>
        /// <param name="callback">The callback which gets called on resolve. Must return an instance</param>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public ServiceDefinition Register<T>(Func<T> callback) => Register(typeof(T), callback);

        /// <summary>
        /// Override any registered service with another implementation. Any singleton instance for this type
        /// will be cleared as well.
        ///
        /// The service itself must not be abstract or an enum.
        /// </summary>
        /// <param name="keyType">The type which gets registered</param>
        /// <param name="serviceType">The concrete implementation type</param>
        /// <exception cref="DependencyRegisterException">If the override fails, it will throw an error</exception>
        public void Override(Type keyType, Type serviceType)
        {
            if (!keyType.IsAssignableFrom(serviceType))
            {
                _logger.Log(LogLevel.Error, $"Service {serviceType} does not extend {keyType}");

                throw new DependencyRegisterException(
                    keyType,
                    $"Service {serviceType} does not extend {keyType}"
                );
            }

            ValidateService(keyType, serviceType);

            if (!_services.ContainsKey(keyType))
            {
                _logger.Log(LogLevel.Error, $"Service {keyType} is not registered");

                throw new DependencyRegisterException(
                    keyType,
                    "Service is not registered. Use `register` to register the service"
                );
            }

            _services[keyType] = new ServiceDefinition(serviceType);

            _logger.Log(LogLevel.Debug, $"Overridden service: {keyType} => {serviceType}");
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
        public void Override<TKey, TService>() where TService : TKey => Override(typeof(TKey), typeof(TService));

        /// <summary>
        /// Override any registered service with a specific instance. The instance will be registered as a singleton
        /// and must extend the given type.
        /// </summary>
        /// <param name="type">The type which gets registered</param>
        /// <param name="instance">The instance which should be returned</param>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public void Override(Type type, object instance)
        {
            Override(type, instance.GetType());
            if (!_services.TryGetValue(instance.GetType(), out var definition))
            {
                _logger.Log(LogLevel.Error, $"Something went wrong with overriding {type}");

                throw new DependencyRegisterException(
                    type,
                    $"Something went wrong with overriding {type}"
                );
            }

            definition.Instance = instance;
        }

        /// <summary>
        /// Override any registered service with a specific instance. The instance will be registered as a singleton
        /// and must extend the given type.
        /// </summary>
        /// <typeparam name="T">The type which gets registered</typeparam>
        /// <param name="instance">The instance which should be returned</param>
        /// <exception cref="DependencyRegisterException">If the registration fails, it will throw an error</exception>
        public void Override<T>(T instance) => Override(typeof(T), instance);

        /// <summary>
        /// Override a service with a callback which gets called on resolve. This is useful if you
        /// want to resolve a service which requires some parameters. The callback will be called on
        /// resolve and be stored as a singleton.
        /// for more information.
        /// </summary>
        /// <param name="keyType">The type which gets registered</param>
        /// <param name="callback">The callback which gets called on resolve. Must return an instance</param>
        /// <exception cref="DependencyRegisterException">If the override fails, it will throw an error</exception>
        public void Override(Type keyType, Func<object> callback)
        {
            if (!_services.ContainsKey(keyType))
            {
                _logger.Log(LogLevel.Error, $"Service {keyType} is not registered");

                throw new DependencyRegisterException(
                    keyType,
                    "Service is not registered. Use `register` to register the service"
                );
            }

            _services[keyType].Instance = null;
            _services[keyType].Callback = callback;

            _logger.Log(LogLevel.Debug, $"Overridden service: {keyType} with callback");
        }

        /// <summary>
        /// Override a service with a callback which gets called on resolve. This is useful if you
        /// want to resolve a service which requires some parameters. The callback will be called on
        /// resolve and be stored as a singleton.
        /// for more information.
        /// </summary>
        /// <typeparam name="T">The type which gets registered</typeparam>
        /// <param name="callback">The callback which gets called on resolve. Must return an instance</param>
        /// <exception cref="DependencyRegisterException">If the override fails, it will throw an error</exception>
        public void Override<T>(Func<T> callback) => Override(typeof(T), () => callback());

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
            _logger.Log(LogLevel.Info, "Clearing container");

            _services.Clear();
            _services.Add(typeof(DucktionLogger), new ServiceDefinition(typeof(DucktionLogger)));

            Reinitialize();
        }

        /// <summary>
        /// Reset every singleton instance. This will not remove the registered services.
        /// If you want to reset everything, use `Clear` instead.
        /// </summary>
        public void ResetSingletons()
        {
            _logger.Log(LogLevel.Info, "Resetting container");

            foreach (var service in _services)
            {
                service.Value.Instance = null;
            }

            Reinitialize();
        }

        /// <summary>
        /// Add a new configurator to the container. This will not execute the configurator, if
        /// the container is already initialized. If you want to reinitialize the container, use
        /// the `Reinitialize` method.
        /// </summary>
        /// <param name="configurator">The new configurator</param>
        public void AddConfigurator(IDiConfigurator configurator)
        {
            _configurators.Add(configurator);
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
            if (!_services.ContainsKey(type) && !enableAutoResolve)
            {
                _logger?.Log(LogLevel.Error, $"Service {type} is not registered");

                throw new DependencyResolveException(type, "Service is not registered");
            }

            if (_services.TryGetValue(type, out var singleton) && singleton.Instance != null)
            {
                return singleton.Instance;
            }

            if (singleton?.Callback != null)
            {
                var result = singleton.Callback();
                StoreAsSingleton(type, result);

                return result;
            }

            var targetType = type;
            var isAutoResolved = true;
            if (_services.TryGetValue(type, out var realType))
            {
                isAutoResolved = false;
                targetType = realType.ServiceType;
            }

            var constructors = targetType.GetConstructors();
            if (constructors.Length > 1)
            {
                _logger?.Log(LogLevel.Error, $"Service {targetType} has multiple constructors");

                throw new DependencyResolveException(targetType, "Service has more than one constructor");
            }

            var parameters = new List<object>();

            foreach (var constructorInfo in constructors)
            {
                foreach (var parameter in constructorInfo.GetParameters())
                {
                    if (dependencyChain.Contains(parameter.ParameterType))
                    {
                        _logger?.Log(LogLevel.Error, $"Service {type} has a circular dependency");

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
                        _logger?.Log(LogLevel.Error, $"Service {type} cant resolve parameter: {parameter.Name}");

                        throw new DependencyResolveException(
                            type,
                            $"Parameter `{parameter.Name}` could not be resolved",
                            exception
                        );
                    }
                }
            }

            var instance = Activator.CreateInstance(targetType, parameters.ToArray());

            if (!isAutoResolved || autoResolveSingletonMode == SingletonMode.Singleton)
            {
                StoreAsSingleton(type, instance);
            }

            _logger?.Log(
                LogLevel.Debug,
                $"Resolved service: {type} => {instance.GetType()}"
            );

            return instance;
        }

        /// <summary>
        /// Register a given instance as a singleton for the given type.
        /// If the type is already registered, it will override the instance.
        /// Otherwise it will create a new service definition.
        /// </summary>
        /// <param name="type">The type which should be registered</param>
        /// <param name="instance">The instance which should be stored as a singleton</param>
        private void StoreAsSingleton(Type type, object instance)
        {
            if (_services.TryGetValue(type, out var definition))
            {
                definition.Instance = instance;
                _services[type] = definition;
                return;
            }

            _services.Add(type, new ServiceDefinition(type) { Instance = instance });
        }

        private void ValidateService(Type keyType, Type serviceType)
        {
            if (serviceType.IsAbstract)
            {
                _logger.Log(LogLevel.Error, $"Service {keyType} is abstract");

                throw new DependencyRegisterException(keyType, "Service is abstract");
            }

            if (serviceType.IsEnum)
            {
                _logger.Log(LogLevel.Error, $"Service {keyType} is an enum");

                throw new DependencyRegisterException(keyType, "Service is an enum");
            }
        }

        /// <summary>
        /// This will initialize all non-lazy services. If a service has no lazy mode specified, it will
        /// default to the `defaultLazyMode` variable.
        /// </summary>
        private void InitializeNonLazyServices()
        {
            (
                _services.Where(service =>
                    (!service.Value.LazyMode.HasValue && defaultLazyMode == LazyMode.NonLazy) ||
                    service.Value?.LazyMode == LazyMode.NonLazy
                ).Select(service => service.Key)
            ).ToList().ForEach(type => Resolve(type));
        }

        #endregion
    }
}