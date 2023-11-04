using TheRealIronDuck.Ducktion.Exceptions;
using UnityEngine;

namespace TheRealIronDuck.Ducktion
{
    /// <summary>
    /// This call can be used as an entrypoint in the package itself. Using `Ducktion.singleton`
    /// you always get the current container instance. If there is no container instance, it will
    /// automatically create one with good defaults.
    /// </summary>
    public class Ducktion
    {
        #region STATIC FIELDS

        /// <summary>
        /// The current container instance. If there is no container instance, it will
        /// create one with good defaults.
        ///
        /// Using: Ducktion.singleton.Register...
        /// </summary>
        public static DiContainer singleton
        {
            get
            {
                if (!_singleton)
                {
                    CreateContainer();
                }

                return _singleton;
            }
        }

        /// <summary>
        /// An internal private reference to the current container.
        /// </summary>
        private static DiContainer _singleton;

        #endregion

        #region PUBLIC STATIC METHODS

        /// <summary>
        /// Register any given container as the new singleton container. This will throw an exception
        /// if there is already a container registered.
        ///
        /// Normally you don't need to use this method manually. The containers will register themselves
        /// in the `Awake` method.
        /// </summary>
        /// <param name="container">The container which should be registered</param>
        /// <exception cref="DucktionException">If there is already a container registered, it will throw an error</exception>
        public static void RegisterContainer(DiContainer container)
        {
            if (_singleton)
            {
                throw new DucktionException(
                    "There is already a container in the scene. You can only have one container at a time."
                );
            }

            _singleton = container;
        }

        /// <summary>
        /// Remove the current container instance. This will also destroy the game object of the container and
        /// all singleton references resolved through it.
        /// </summary>
        public static void Clear()
        {
            if (_singleton)
            {
                _singleton.Clear();

                if (Application.isPlaying)
                {
                    Object.Destroy(_singleton.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(_singleton.gameObject);
                }
            }

            _singleton = null;
        }

        #endregion

        #region PRIVATE STATIC METHODS

        /// <summary>
        /// Create a blank container instance and register it as the singleton.
        /// </summary>
        private static void CreateContainer()
        {
            var gameObject = new GameObject("Ducktion Container");
            _singleton = gameObject.AddComponent<DiContainer>();
        }

        #endregion
    }
}