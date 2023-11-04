using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;

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
    }
}