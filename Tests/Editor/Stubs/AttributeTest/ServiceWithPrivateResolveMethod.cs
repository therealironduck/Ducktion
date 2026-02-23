using TheRealIronDuck.Ducktion.Attributes;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithPrivateResolveMethod
    {
        public SimpleService Simple { get; private set; }
        public AnotherService Another { get; private set; }

        [Resolve]
        private void ThisIsAMethodForSure(SimpleService simple, AnotherService another, DucktionLogger logger)
        {
            logger.Log(LogLevel.Debug, "I was called!");

            Simple = simple;
            Another = another;
        }
    }
}
