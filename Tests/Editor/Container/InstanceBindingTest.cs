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
    }
}