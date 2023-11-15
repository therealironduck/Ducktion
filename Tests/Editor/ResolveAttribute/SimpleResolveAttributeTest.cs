using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.ResolveAttribute
{
    public class SimpleResolveAttributeTest : DucktionTest
    {
        [Test]
        public void ItResolvesAnyPublicFieldWithAResolveAttributeWhenResolvingTheMainService()
        {
            // Register registered services
            container.Register<SimpleService>();
            container.Register<AnotherService>();
            container.Register<ServiceWithPublicAttribute>();
            
            // Resolve the main service
            var service = container.Resolve<ServiceWithPublicAttribute>();
            
            // Ensure that both the resolve attribute and the constructor parameter are resolved
            Assert.NotNull(service.Simple);
            Assert.NotNull(service.Another);
        }
        
        [Test]
        public void ItResolvesPrivateAndProtectedFieldsAsWell()
        {
            // Register registered services
            container.Register<SimpleService>();
            container.Register<AnotherService>();
            container.Register<ServiceWithPrivateAndProtectedAttribute>();
            
            // Resolve the main service
            var service = container.Resolve<ServiceWithPrivateAndProtectedAttribute>();
            
            // Ensure that both the resolve attribute and the constructor parameter are resolved
            Assert.NotNull(service.Simple);
            Assert.NotNull(service.AnotherService);
        }
        
        [Test]
        public void ItResolvesProperties()
        {
            // Register registered services
            container.Register<SimpleService>();
            container.Register<ServiceWithProperty>();
            
            // Resolve the main service
            var service = container.Resolve<ServiceWithProperty>();
            
            // Ensure that both the resolve attribute and the constructor parameter are resolved
            Assert.NotNull(service.Simple);
        }
        
        // TEST IT can resolve whole method parameters
        // TEST IT can specify the ID for variables
        // TEST IT can specify the ID for constructor arguments
        // TEST IT can specify the ID for method parameters
    }
}