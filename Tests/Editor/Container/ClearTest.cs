using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class ClearTest : DucktionTest
    {
        [Test]
        public void ItCanClearAContainer()
        {
            container.Register<SimpleService>();

            var service = container.Resolve<SimpleService>();
            Assert.NotNull(service);

            container.Clear();
            Assert.Throws<DependencyResolveException>(() => container.Resolve<SimpleService>());
        }
        
        [Test]
        public void ItClearsAllSingletonInstances()
        {
            container.Register<SimpleService>();
            var serviceA = container.Resolve<SimpleService>();

            container.Clear();
            
            container.Register<SimpleService>();
            var serviceB = container.Resolve<SimpleService>();
            
            Assert.AreNotEqual(serviceA, serviceB);
        }
        
        [Test]
        public void ITCanOnlyResetTheSingletons()
        {
            container.Register<SimpleService>();
            var serviceA = container.Resolve<SimpleService>();

            container.Reset();
            
            var serviceB = container.Resolve<SimpleService>();
            
            Assert.AreNotEqual(serviceA, serviceB);
        }
    }
}