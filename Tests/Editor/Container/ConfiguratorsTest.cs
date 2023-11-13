using System.Reflection;
using NUnit.Framework;
using TheRealIronDuck.Ducktion.Configurators;
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
        
        // [Test]
        // public void ItRunsAllRegisteredMonoConfigurators()
        // {
        //     var monoConfigurator = container.gameObject.AddComponent<ExampleMonoConfigurator>();
        //     
        //     var containerType = container.GetType();
        //     var monoField = containerType.GetField(
        //         "defaultConfigurators",
        //         BindingFlags.NonPublic | BindingFlags.Instance
        //     );
        //     
        //     monoField?.SetValue(container, new MonoDiConfigurator[]
        //     {
        //         monoConfigurator
        //     });
        //     
        //     container.Reinitialize();
        //     
        //     Assert.IsTrue(monoConfigurator.Called);
        //     
        //     Assert.NotNull(container.Resolve<ISimpleInterface>());
        //     Assert.NotNull(container.Resolve<ScalarService>());
        //     Assert.NotNull(container.Resolve<AnotherService>());
        // }

    }
}