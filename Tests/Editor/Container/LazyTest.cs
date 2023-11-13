using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class LazyTest : DucktionTest
    {
        [Test]
        public void ItCanRegisterAServiceAsNonLazy()
        {
            var logger = FakeLogger();

            container.Register<ServiceWithLogger>().NonLazy();
            container.Register<SecondServiceWithLogger>();
            container.Reinitialize();

            logger.AssertHasMessage(LogLevel.Debug, "Hello from ServiceWithLogger!");
            logger.AssertHasNoMessage(LogLevel.Debug, "Hello from SecondServiceWithLogger!");
        }
        
        // TODO: Default can be non lazy
        // TODO: Default can be non lazy, but I register a lazy service
        // TODO: Test alias `LazyMode`
        // TODO: Test every register method syntax
        // TODO: Test override
        // TODO: Test every override method syntax
    }
}