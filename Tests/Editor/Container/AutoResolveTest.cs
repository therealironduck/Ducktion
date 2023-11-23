using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Enums;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class AutoResolveTest : DucktionTest
    {
        protected override DucktionTestConfig Configure()
        {
            return new DucktionTestConfig(enableAutoResolve: true);
        }

        [Test]
        public void ItCanAutomaticallyResolveUnknownServices()
        {
            var result = container.Resolve<SimpleService>();
            Assert.NotNull(result);
            Assert.IsInstanceOf<SimpleService>(result);
        }
        
        [Test]
        public void ItCanAutomaticallyResolveUnknownServicesRecursively()
        {
            var result = container.Resolve<SimpleServiceWithDependency>();
            Assert.NotNull(result);
            Assert.IsInstanceOf<SimpleServiceWithDependency>(result);
            
            Assert.IsInstanceOf<AnotherService>(result.Another);
        }
        
        [Test]
        public void ItCanMixAutomaticResolvesWithManuallyRegisteredInterfaces()
        {
            container.Register<ISimpleInterface, SimpleServiceWithDependency>();
            
            var result = container.Resolve<ServiceWithDependencies>();
            Assert.NotNull(result);
            Assert.IsInstanceOf<ServiceWithDependencies>(result);
            
            Assert.IsInstanceOf<SimpleServiceWithDependency>(result.Simple);
            
            Assert.IsInstanceOf<AnotherService>((result.Simple as SimpleServiceWithDependency)?.Another);
        }
        
        [Test]
        public void ItStoresThemAsSingletons()
        {
            var result1 = container.Resolve<SimpleService>();
            var result2 = container.Resolve<SimpleService>();
            
            Assert.AreSame(result1, result2);
        }
        
        [Test]
        public void ItCanOptionallyNotStoreThemAsSingletons()
        {
            container.Configure(newAutoResolveSingletonMode: SingletonMode.NonSingleton);
            
            var result1 = container.Resolve<SimpleService>();
            var result2 = container.Resolve<SimpleService>();
            
            Assert.AreNotSame(result1, result2);
        }
    }
}