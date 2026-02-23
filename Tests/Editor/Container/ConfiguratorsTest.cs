using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class ConfiguratorsTest : DucktionTest
    {
        [Test]
        public void ItRunsEveryRegisteredConfiguratorOnStartup()
        {
            var configurator = new ExampleConfigurator();

            container.AddConfigurator(configurator);
            container.Reinitialize();
            
            Assert.IsTrue(configurator.Called);
            
            Assert.NotNull(container.Resolve<ISimpleInterface>());
            Assert.NotNull(container.Resolve<ScalarService>());
            Assert.NotNull(container.Resolve<AnotherService>());
        }
    }
}
