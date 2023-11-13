using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Enums;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor
{
    public class ServiceDefinitionTest : DucktionTest
    {
        [Test]
        public void ItCreatesAServiceDefinitionWithDefaultValues()
        {
            var definition = container.Register<SimpleService>();
            Assert.That(definition.ServiceType, Is.EqualTo(typeof(SimpleService)));
            Assert.That(definition.LazyMode, Is.Null);
            Assert.That(definition.Instance, Is.Null);
            Assert.That(definition.Callback, Is.Null);
        }

        [Test]
        public void ItCanToggleTheLazyMode()
        {
            var definition = container.Register<SimpleService>();
            definition.NonLazy();
            Assert.That(definition.LazyMode, Is.EqualTo(Enums.LazyMode.NonLazy));
            
            definition.Lazy();
            Assert.That(definition.LazyMode, Is.EqualTo(Enums.LazyMode.Lazy));
            
            definition.SetLazyMode(LazyMode.NonLazy);
            Assert.That(definition.LazyMode, Is.EqualTo(Enums.LazyMode.NonLazy));
        }

        [Test]
        public void ItCanFluentlyChangeTheLazyMode()
        {
            var definition = container.Register<SimpleService>();
            definition.NonLazy().Lazy();
            
            Assert.That(definition.LazyMode, Is.EqualTo(Enums.LazyMode.Lazy));
        }
    }
}