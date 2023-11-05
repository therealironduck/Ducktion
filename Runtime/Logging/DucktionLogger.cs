using UnityEngine;

namespace TheRealIronDuck.Ducktion.Logging
{
    public class DucktionLogger
    {
        #region VARIABLES

        private LogLevel _level = LogLevel.Error;

        #endregion

        #region PUBLIC METHODS

        public void Configure(LogLevel level)
        {
            _level = level;
        }

        public virtual void Log(LogLevel level, string message)
        {
            if (_level > level)
            {
                return;
            }

            switch (level)
            {
                case LogLevel.Error:
                    Debug.LogError($"[Ducktion] [{level}] {message}");
                    break;

                case LogLevel.Debug:
                case LogLevel.Info:
                default:
                    Debug.Log($"[Ducktion] [{level}] {message}");
                    break;
            }
        }

        #endregion
    }
}