using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class InstanceBindingTest : DucktionTest
    {
        [Test]
        public void ItCanRegisterSpecificInstances()
        {
            var service = new SimpleService();

            container.Register<ISimpleInterface>(service);

            Assert.AreSame(service, container.Resolve<ISimpleInterface>());
        }

        [Test]
        public void ItCanRegisterSpecificInstancesUsingTypeParameter()
        {
            var service = new SimpleService();

            container.Register(typeof(ISimpleInterface), service);

            Assert.AreSame(service, container.Resolve<ISimpleInterface>());
        }

        [Test]
        public void ItThrowsAnExceptionIfTheSpecificInstanceDoesntExtendTheTypeParameter()
        {
            var service = new AnotherService();

            var error = Assert.Throws<DependencyRegisterException>(
                () => container.Register(typeof(ISimpleInterface), service)
            );

            Assert.That(error.Message, Does.Contain(
                $"Service {typeof(AnotherService)} does not extend {typeof(ISimpleInterface)}"
            ));
        }
        
        [Test]
        public void ItAllowsToRegisterForExampleGameObjectsWhichHaveMultipleConstructors()
        {
            var gObj = new GameObject("Hello World!");
            container.Register<GameObject>(gObj);
            
            Assert.AreSame(gObj, container.Resolve<GameObject>());
        }

        [Test]
        public void ItCanOverrideAServiceWithASpecificInstance()
        {
            container.Register<SimpleService>();
            
            var old = container.Resolve<SimpleService>();
            
            var service = new SimpleService();
            container.Override<SimpleService>(service);
            
            var current = container.Resolve<SimpleService>();
            
            Assert.AreSame(service, current);
            Assert.AreNotSame(old, current);
        }

        [Test]
        public void ItCanOverrideAnExistingInstance()
        {
            var serviceA = new SimpleService();
            var serviceB = new SimpleService();
            
            container.Register<SimpleService>(serviceA);
            container.Override<SimpleService>(serviceB);
            
            var current = container.Resolve<SimpleService>();
            
            Assert.AreSame(serviceB, current);
        }

        [Test]
        public void ItCanOverrideUsingTypeParameters()
        {
            container.Register<SimpleService>();
            
            var old = container.Resolve<SimpleService>();
            
            var service = new SimpleService();
            container.Override(typeof(SimpleService), service);
            
            var current = container.Resolve<SimpleService>();
            
            Assert.AreSame(service, current);
            Assert.AreNotSame(old, current);
        }
    }
}