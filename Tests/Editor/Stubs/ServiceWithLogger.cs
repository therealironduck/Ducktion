using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs
{
    public class ServiceWithLogger
    {
        public ServiceWithLogger(DucktionLogger logger)
        {
            logger.Log(LogLevel.Debug, "Hello from ServiceWithLogger!");
        }
    }
}