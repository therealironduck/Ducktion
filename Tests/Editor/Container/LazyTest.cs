using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Enums;
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
        
        [Test]
        public void ItCanSetTheDefaultToNonLazy()
        {
            var logger = FakeLogger();
            
            container.Configure(newDefaultLazyMode: LazyMode.NonLazy);

            container.Register<ServiceWithLogger>();
            container.Register<SecondServiceWithLogger>();
            container.Reinitialize();

            logger.AssertHasMessage(LogLevel.Debug, "Hello from ServiceWithLogger!");
            logger.AssertHasMessage(LogLevel.Debug, "Hello from SecondServiceWithLogger!");
        }
        
        [Test]
        public void ItCanSetTheDefaultToNonLazyButRegisterSpecificServicesAsLazy()
        {
            var logger = FakeLogger();
            
            container.Configure(newDefaultLazyMode: LazyMode.NonLazy);

            container.Register<ServiceWithLogger>().Lazy();
            container.Register<SecondServiceWithLogger>();
            container.Reinitialize();

            logger.AssertHasNoMessage(LogLevel.Debug, "Hello from ServiceWithLogger!");
            logger.AssertHasMessage(LogLevel.Debug, "Hello from SecondServiceWithLogger!");
        }
        
        [Test]
        public void ItCanSetTheSpecificLazyModeWithoutANiceMethod()
        {
            var logger = FakeLogger();
            
            container.Configure(newDefaultLazyMode: LazyMode.NonLazy);

            container.Register<ServiceWithLogger>().SetLazyMode(LazyMode.Lazy);
            container.Register<SecondServiceWithLogger>().SetLazyMode(LazyMode.NonLazy);
            container.Reinitialize();

            logger.AssertHasNoMessage(LogLevel.Debug, "Hello from ServiceWithLogger!");
            logger.AssertHasMessage(LogLevel.Debug, "Hello from SecondServiceWithLogger!");
        }
        
        // TODO: Test every register method syntax
        // TODO: Test override
        // TODO: Test every override method syntax
        // TODO: Test chaining
    }
}