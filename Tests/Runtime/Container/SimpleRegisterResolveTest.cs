using NUnit.Framework;
using TheRealIronDuck.Ducktion.Exceptions;
using TheRealIronDuck.Ducktion.Tests.Stubs;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Tests.Container
{
    public class SimpleRegisterResolveTest
    {
        [Test]
        public void ItCanRegisterASimpleServiceAndResolveIt()
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();
            container.Register<SimpleService>();
            
            var service = container.Resolve<SimpleService>();
            Assert.IsInstanceOf<SimpleService>(service);
        }
        
        [Test]
        public void ItCanRegisterAServiceForAnInterfaceAndResolveIt()
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();
            container.Register<ISimpleInterface, SimpleService>();
            
            var service = container.Resolve<ISimpleInterface>();
            Assert.IsInstanceOf<SimpleService>(service);
        }
        
        [Test]
        public void ItCanRegisterAServiceForAnParentClassAndResolveIt()
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();
            container.Register<SimpleBaseClass, SimpleService>();
            
            var service = container.Resolve<SimpleBaseClass>();
            Assert.IsInstanceOf<SimpleService>(service);
        }
        
        [Test]
        public void ItThrowsAnErrorIfTheServiceIsUnknown()
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();

            Assert.Throws<DependencyResolveException>(() =>
            {
                container.Resolve<SimpleService>();
            });
        }
        
        [Test]
        public void ItThrowsAnErrorIfTheRegisteredServiceIsAnAbstractClass()
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();

            Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Register<SimpleBaseClass>();
            });
        }
        
        [Test]
        public void ItThrowsAnErrorIfTheRegisteredServiceIsAnInterface()
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();

            Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Register<ISimpleInterface>();
            });
        }
        
        [Test]
        public void ItThrowsAnErrorIfTheRegisteredServiceIsAnEnum()
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();

            Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Register<SimpleEnum>();
            });
        }
    }
}