using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Attributes.ResolveTags
{
    public class ManualResolveTagsAttributeTest : DucktionTest
    {
        [Test]
        public void ItCanResolveAnyTaggedServicesAfterObjectAlreadyExists()
        {
            container.Register<SimpleService>().WithTag("example");
            container.Register<AnotherService>().WithTag("example");
            
            var service = new ServiceWithResolveMethodAndResolveTags();
            container.ResolveDependencies(service);
            
            Assert.NotNull(service.Simple);
            Assert.AreEqual(2, service.Simple.Count);
        }
    }
}
