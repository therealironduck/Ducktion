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
            Assert.That(definition.SingletonMode, Is.Null);
            Assert.That(definition.Instance, Is.Null);
            Assert.That(definition.Callback, Is.Null);
            Assert.That(definition.Id, Is.Null);
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

        [Test]
        public void ItCanToggleTheSingletonMode()
        {
            var definition = container.Register<SimpleService>();
            definition.NonSingleton();
            Assert.That(definition.SingletonMode, Is.EqualTo(Enums.SingletonMode.NonSingleton));
            
            definition.Singleton();
            Assert.That(definition.SingletonMode, Is.EqualTo(Enums.SingletonMode.Singleton));

            definition.Transient();
            Assert.That(definition.SingletonMode, Is.EqualTo(Enums.SingletonMode.NonSingleton));
            
            definition.SetSingletonMode(SingletonMode.Singleton);
            Assert.That(definition.SingletonMode, Is.EqualTo(Enums.SingletonMode.Singleton));
        }
        
        [Test]
        public void ItCanFluentlyChangeTheSingletonMode()
        {
            var definition = container.Register<SimpleService>();
            definition.Singleton().Lazy();
            
            Assert.That(definition.SingletonMode, Is.EqualTo(Enums.SingletonMode.Singleton));
            Assert.That(definition.LazyMode, Is.EqualTo(Enums.LazyMode.Lazy));
        }

        [Test]
        public void ItCanSetTheInstance()
        {
            var instance = new SimpleService();
            
            var definition = container.Register<SimpleService>();
            definition.SetInstance(instance);
            Assert.That(definition.Instance, Is.SameAs(instance));
            
            definition.SetInstance(null);
            Assert.That(definition.Instance, Is.Null);
        }
        
        [Test]
        public void ItCanFluentlySetTheInstance()
        {
            var instance = new SimpleService();
            
            var definition = container.Register<SimpleService>();
            definition.SetInstance(instance).Lazy();
            Assert.That(definition.Instance, Is.SameAs(instance));
            Assert.That(definition.LazyMode, Is.EqualTo(LazyMode.Lazy));
        }
    }
}