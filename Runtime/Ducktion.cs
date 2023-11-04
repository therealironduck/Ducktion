using TheRealIronDuck.Ducktion.Exceptions;
using UnityEngine;

namespace TheRealIronDuck.Ducktion
{
    public class Ducktion
    {
        #region STATIC FIELDS

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

        private static DiContainer _singleton;

        #endregion

        #region PUBLIC STATIC METHODS

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

        private static void CreateContainer()
        {
            var gameObject = new GameObject("Ducktion Container");
            _singleton = gameObject.AddComponent<DiContainer>();
        }

        #endregion
    }
}