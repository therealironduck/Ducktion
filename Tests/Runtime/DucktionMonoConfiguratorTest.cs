using System;
using System.Reflection;
using NUnit.Framework;
using TheRealIronDuck.Ducktion.Configurators;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Tests.Stubs;

namespace TheRealIronDuck.Ducktion.Tests
{
    public class DucktionMonoConfiguratorTest : DucktionTest
    {
        protected override DucktionTestConfig Configure() => new(
            createContainer: false
        );
        
        [Test]
        public void ItRunsAllRegisteredMonoConfigurators()
        {
            var singletonContainer = Ducktion.singleton;
            var monoConfigurator = singletonContainer.gameObject.AddComponent<ExampleMonoConfigurator>();
            
            var containerType = singletonContainer.GetType();
            var monoField = containerType.GetField(
                "defaultConfigurators",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            
            monoField?.SetValue(singletonContainer, new MonoDiConfigurator[]
            {
                monoConfigurator
            });

            // This is a small hack, but basically we call the Awake method again.
            // In a normal scenario, this would be called by Unity when the game starts.
            // The default mono configurators would be set in editor so they would be available
            // when the Awake method is called.
            var awake = containerType.GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
            awake?.Invoke(singletonContainer, Array.Empty<object>());
            
            Assert.IsTrue(monoConfigurator.called);
            
            Assert.NotNull(singletonContainer.Resolve<ISimpleInterface>());
            Assert.NotNull(singletonContainer.Resolve<ScalarService>());
            Assert.NotNull(singletonContainer.Resolve<AnotherService>());
        }
    }
}