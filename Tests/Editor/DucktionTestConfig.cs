using TheRealIronDuck.Ducktion.Enums;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor
{
    public struct DucktionTestConfig
    {
        public readonly bool CreateContainer;
        public readonly LogLevel LogLevel;
        public readonly bool EnableAutoResolve;
        public readonly SingletonMode AutoResolveSingletonMode;
        public readonly bool EnableEventBus;

        public DucktionTestConfig(
            bool createContainer = true,
            LogLevel logLevel = LogLevel.Disabled,
            bool enableAutoResolve = false,
            SingletonMode autoResolveSingletonMode = SingletonMode.Singleton,
            bool enableEventBus = false
        )
        {
            EnableAutoResolve = enableAutoResolve;
            AutoResolveSingletonMode = autoResolveSingletonMode;
            CreateContainer = createContainer;
            LogLevel = logLevel;
            EnableEventBus = enableEventBus;
        }
    }
}