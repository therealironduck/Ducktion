using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Enums;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class SingletonTest : DucktionTest
    {
        [Test]
        public void ItRegistersAnyServiceAsSingletonByDefault()
        {
            // We register any service
            container.Register<SimpleService>();

            // We resolve the service twice
            var service1 = container.Resolve<SimpleService>();
            var service2 = container.Resolve<SimpleService>();

            // We check if both services are the same
            Assert.AreSame(service1, service2);
        }

        [Test]
        public void ItCanRegisterSomeServicesAsNonSingleton()
        {
            // We register any service
            container.Register<SimpleService>().NonSingleton();

            // We resolve the service twice
            var service1 = container.Resolve<SimpleService>();
            var service2 = container.Resolve<SimpleService>();

            // We check if both services are NOT the same
            Assert.AreNotSame(service1, service2);
        }

        [Test]
        public void ItCanChangeTheDefaultModeToNonSingleton()
        {
            container.Configure(newDefaultSingletonMode: SingletonMode.NonSingleton);

            // We register any service
            container.Register<SimpleService>();

            // We resolve the service twice
            var service1 = container.Resolve<SimpleService>();
            var service2 = container.Resolve<SimpleService>();

            // We check if both services are NOT the same
            Assert.AreNotSame(service1, service2);
        }

        [Test]
        public void ItCanRegisterCallbackBasedServicesAsNonSingleton()
        {
            // We register any service
            container.Register<SimpleService>().SetCallback(() => new SimpleService()).NonSingleton();

            // We resolve the service twice
            var service1 = container.Resolve<SimpleService>();
            var service2 = container.Resolve<SimpleService>();

            // We check if both services are NOT the same
            Assert.AreNotSame(service1, service2);
        }

        [Test]
        public void ItCanRegisterCallbackBasedServicesAsNonSingletonWithContainerDefaults()
        {
            container.Configure(newDefaultSingletonMode: SingletonMode.NonSingleton);

            // We register any service
            container.Register<SimpleService>().SetCallback(() => new SimpleService());

            // We resolve the service twice
            var service1 = container.Resolve<SimpleService>();
            var service2 = container.Resolve<SimpleService>();

            // We check if both services are NOT the same
            Assert.AreNotSame(service1, service2);
        }

        [Test]
        public void ItThrowsAnErrorIfAnInstanceBindIsNonSingleton()
        {
            var error = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Register<SimpleService>().SetInstance(new SimpleService()).NonSingleton();
            });

            Assert.That(error.Message, Does.Contain("Cannot bind an instance as non singleton"));
        }
    }
}