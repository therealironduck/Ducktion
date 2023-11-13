using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs
{
    public class SecondServiceWithLogger
    {
        public SecondServiceWithLogger(DucktionLogger logger)
        {
            logger.Log(LogLevel.Debug, "Hello from SecondServiceWithLogger!");
        }
    }
}