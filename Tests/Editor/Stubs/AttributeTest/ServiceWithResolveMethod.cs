using TheRealIronDuck.Ducktion.Logging;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithResolveMethod
    {
        public SimpleService Simple { get; private set; }
        public AnotherService Another { get; private set; }

        [Attributes.Resolve]
        public void ThisIsAMethodForSure(SimpleService simple, AnotherService another, DucktionLogger logger)
        {
            logger.Log(LogLevel.Debug, "I was called!");

            Simple = simple;
            Another = another;
        }
    }
}