using TheRealIronDuck.Ducktion.Attributes;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Components
{
    /// <summary>
    /// This component can be attached to a GameObject to resolve all dependencies marked with the
    /// <see cref="ResolveAttribute"/>. This is useful when you want to resolve dependencies on
    /// game-objects that are created at runtime, like prefabs.
    /// </summary>
    public class DynamicDependencyResolver : MonoBehaviour
    {
        /// <summary>
        /// This method only calls the container to request a dependency resolution.
        /// </summary>
        private void Awake()
        {
            Ducktion.singleton.ResolveDependencies(gameObject);
        }
    }
}