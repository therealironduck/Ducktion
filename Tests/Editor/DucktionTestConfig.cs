using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor
{
    public struct DucktionTestConfig
    {
        public readonly bool CreateContainer;
        public readonly LogLevel LogLevel;

        public DucktionTestConfig(bool createContainer = true, LogLevel logLevel = LogLevel.Disabled)
        {
            CreateContainer = createContainer;
            LogLevel = logLevel;
        }
    }
}