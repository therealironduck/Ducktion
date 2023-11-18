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

        [Test]
        public void ItReturnsTheServiceDefinitionForEveryPossibleRegisterSyntax()
        {
            Assert.That(
                container.Register(typeof(ISimpleInterface), typeof(SimpleService)),
                Is.InstanceOf<ServiceDefinition>()
            );

            Assert.That(
                container.Register(typeof(AnotherService)),
                Is.InstanceOf<ServiceDefinition>()
            );

            Assert.That(
                container.Register<ScalarService>(),
                Is.InstanceOf<ServiceDefinition>()
            );

            Assert.That(
                container.Register<SimpleBaseClass, SimpleService>(),
                Is.InstanceOf<ServiceDefinition>()
            );
            
            container.Clear();
            
            Assert.That(
                container.Register(typeof(SimpleService)).SetCallback(() => new SimpleService()),
                Is.InstanceOf<ServiceDefinition>()
            );
            
            Assert.That(
                container.Register<ISimpleInterface, SimpleService>().SetCallback(() => new SimpleService()),
                Is.InstanceOf<ServiceDefinition>()
            );
        }

        [Test]
        public void ItCanMarkAServiceAfterwardsAsLazy()
        {
            var logger = FakeLogger();

            container.Register<ServiceWithLogger>().Lazy();
            container.Override<ServiceWithLogger, ServiceWithLogger>().NonLazy();
            
            container.Reinitialize();

            logger.AssertHasMessage(LogLevel.Debug, "Hello from ServiceWithLogger!");
        }
        
        [Test]
        public void ItReturnsTheServiceDefinitionForEveryPossibleOverrideSyntax()
        {
            container.Register<ISimpleInterface, SimpleService>();
            
            Assert.That(
                container.Override(typeof(ISimpleInterface), typeof(SimpleService)),
                Is.InstanceOf<ServiceDefinition>()
            );
            
            Assert.That(
                container.Override<ISimpleInterface, SimpleService>(),
                Is.InstanceOf<ServiceDefinition>()
            );
            
            Assert.That(
                container.Override(typeof(ISimpleInterface)).SetCallback(() => new SimpleService()),
                Is.InstanceOf<ServiceDefinition>()
            );
            
            Assert.That(
                container.Override<ISimpleInterface>().SetCallback(() => new SimpleService()),
                Is.InstanceOf<ServiceDefinition>()
            );
        }
    }
}