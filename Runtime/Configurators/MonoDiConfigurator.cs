using UnityEngine;

namespace TheRealIronDuck.Ducktion.Configurators
{
    /// <summary>
    /// This base class for configurators allows you to register your dependencies
    /// in the dependency injection container while also extending MonoBehaviour.
    /// This is useful if you want to use the Unity Editor to configure your dependencies.
    /// </summary>
    public abstract class MonoDiConfigurator : MonoBehaviour, IDiConfigurator
    {
        /// <summary>
        /// In this method you may use the container to register your dependencies.
        /// Please note that you should not use the container to resolve dependencies at
        /// this stage, as it may not be fully configured yet.
        /// </summary>
        /// <param name="container">The dependency injection container</param>
        public abstract void Register(DiContainer container);
    }
}