using System;
using JetBrains.Annotations;
using TheRealIronDuck.Ducktion.Enums;

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
        /// The singleton instance of the service. Can be null if the service is not a singleton
        /// or if the service has not been resolved yet.
        ///
        /// Can only be set by the container.
        /// </summary>
        [CanBeNull] public object Instance { get; internal set; }

        /// <summary>
        /// The given callback to resolve the service. Can be null if no callback was given.
        ///
        /// Can only be set by the container.
        /// </summary>
        [CanBeNull] public Func<object> Callback { get; internal set; }

        /// <summary>
        /// Specify if the service should be resolved lazily or not. By default, no lazy mode
        /// is specified (null), which means that the container will use the default lazy mode.
        /// </summary>
        public LazyMode? LazyMode { get; private set; }

        #endregion

        #region LIFECYCLE METHODS

        internal ServiceDefinition(Type serviceType)
        {
            ServiceType = serviceType;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Mark this service as non lazy.
        /// </summary>
        public ServiceDefinition NonLazy() => SetLazyMode(Enums.LazyMode.NonLazy);

        /// <summary>
        /// Mark this service as lazy.
        /// </summary>
        public ServiceDefinition Lazy() => SetLazyMode(Enums.LazyMode.Lazy);

        /// <summary>
        /// Set the lazy mode of this service.
        /// </summary>
        /// <param name="lazyMode">The new lazy mode</param>
        public ServiceDefinition SetLazyMode(LazyMode lazyMode)
        {
            LazyMode = lazyMode;
            return this;
        }

        #endregion

    }
}