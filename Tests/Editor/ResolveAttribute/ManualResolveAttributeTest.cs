using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.ResolveAttribute
{
    public class ManualResolveAttributeTest : DucktionTest
    {
        [Test]
        public void ItCanResolveAnyVariablesAfterTheObjectAlreadyExists()
        {
            var logger = FakeLogger();

            container.Register<SimpleService>();
            container.Register<AnotherService>();
            
            var service = new ServiceWithResolveMethod();
            logger.AssertHasNoMessage(LogLevel.Debug, "I was called!");
            
            container.ResolveDependencies(service);
            
            logger.AssertHasMessage(LogLevel.Debug, "I was called!");
            Assert.NotNull(service.Simple);
            Assert.NotNull(service.Another);
        }
    }
}