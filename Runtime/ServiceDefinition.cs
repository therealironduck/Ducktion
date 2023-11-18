using System;
using JetBrains.Annotations;
using TheRealIronDuck.Ducktion.Enums;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion
{
    /// <summary>
    /// This class hold all the information needed to resolve a service.
    /// Most variables can't be set directly by the user, but only by the container.
    /// </summary>
    public class ServiceDefinition
    {
        #region VARIABLES

        /// <summary>
        /// The type of the service. This is the variable that must be passed to the
        /// `Resolve` method of the container class.
        /// </summary>
        public readonly Type ServiceType;

        /// <summary>
        /// The service id. This can be used to register multiple services of the same type.
        /// </summary>
        [CanBeNull] public readonly string Id;

        /// <summary>
        /// The singleton instance of the service. Can be null if the service is not a singleton
        /// or if the service has not been resolved yet.
        ///
        /// Can only be set by the container.
        /// </summary>
        [CanBeNull]
        public object Instance { get; private set; }

        /// <summary>
        /// The given callback to resolve the service. Can be null if no callback was given.
        ///
        /// Can only be set by the container.
        /// </summary>
        [CanBeNull]
        public Func<object> Callback { get; private set; }

        /// <summary>
        /// Specify if the service should be resolved lazily or not. By default, no lazy mode
        /// is specified (null), which means that the container will use the default lazy mode.
        /// </summary>
        public LazyMode? LazyMode { get; private set; }

        /// <summary>
        /// Specify if the service should be stored as a singleton or not. By default, no singleton
        /// mode is specified (null), which means that the container will use the default singleton mode.
        /// </summary>
        public SingletonMode? SingletonMode { get; private set; }

        #endregion

        #region LIFECYCLE METHODS

        internal ServiceDefinition(Type serviceType, [CanBeNull] string id)
        {
            ServiceType = serviceType;
            Id = id;
        }

        #endregion

        #region LAZY CONFIGURATION

        /// <summary>
        /// Set the lazy mode of this service.
        /// </summary>
        /// <param name="lazyMode">The new lazy mode</param>
        public ServiceDefinition SetLazyMode(LazyMode lazyMode)
        {
            LazyMode = lazyMode;
            return this;
        }

        /// <summary>
        /// Mark this service as non lazy.
        /// </summary>
        public ServiceDefinition NonLazy() => SetLazyMode(Enums.LazyMode.NonLazy);

        /// <summary>
        /// Mark this service as lazy.
        /// </summary>
        public ServiceDefinition Lazy() => SetLazyMode(Enums.LazyMode.Lazy);

        #endregion

        #region SINGLETON CONFIGURATION

        /// <summary>
        /// Set the singleton mode of this service.
        /// </summary>
        /// <param name="singletonMode">The new singleton mode</param>
        public ServiceDefinition SetSingletonMode(SingletonMode singletonMode)
        {
            if (Instance != null && singletonMode == Enums.SingletonMode.NonSingleton)
            {
                throw new DependencyRegisterException(
                    ServiceType,
                    "Cannot bind an instance as non singleton"
                );
            }

            SingletonMode = singletonMode;

            return this;
        }

        /// <summary>
        /// Mark this service as non singleton
        /// </summary>
        public ServiceDefinition NonSingleton() => SetSingletonMode(Enums.SingletonMode.NonSingleton);

        /// <summary>
        /// Mark this service as singleton
        /// </summary>
        public ServiceDefinition Singleton() => SetSingletonMode(Enums.SingletonMode.Singleton);

        /// <summary>
        /// Mark this service as non singleton. Alias for `NonSingleton`.
        /// </summary>
        public ServiceDefinition Transient() => SetSingletonMode(Enums.SingletonMode.NonSingleton);

        #endregion

        #region INSTANCE CONFIGURATION

        public ServiceDefinition SetInstance([CanBeNull] object instance)
        {
            if (!ServiceType.IsInstanceOfType(instance) && instance != null)
            {
                throw new DependencyRegisterException(
                    ServiceType,
                    $"Service {instance.GetType()} does not extend {ServiceType}"
                );
            }

            Instance = instance;

            return this;
        }

        #endregion

        #region CALLBACK CONFIGURATION

        public ServiceDefinition SetCallback(Func<object> callback)
        {
            Instance = null;
            Callback = callback;

            return this;
        }

        #endregion
    }
}