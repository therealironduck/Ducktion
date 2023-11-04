using NUnit.Framework;
using TheRealIronDuck.Ducktion.Exceptions;
using TheRealIronDuck.Ducktion.Tests.Stubs;

namespace TheRealIronDuck.Ducktion.Tests.Container
{
    public class SimpleRegisterResolveTest : DucktionTest
    {
        [Test]
        public void ItCanRegisterASimpleServiceAndResolveIt()
        {
            container.Register<SimpleService>();

            var service = container.Resolve<SimpleService>();
            Assert.IsInstanceOf<SimpleService>(service);
        }

        [Test]
        public void ItCanRegisterAServiceForAnInterfaceAndResolveIt()
        {
            container.Register<ISimpleInterface, SimpleService>();

            var service = container.Resolve<ISimpleInterface>();
            Assert.IsInstanceOf<SimpleService>(service);
        }

        [Test]
        public void ItCanRegisterAServiceForAnParentClassAndResolveIt()
        {
            container.Register<SimpleBaseClass, SimpleService>();

            var service = container.Resolve<SimpleBaseClass>();
            Assert.IsInstanceOf<SimpleService>(service);
        }

        [Test]
        public void ItThrowsAnErrorIfTheServiceIsUnknown()
        {
            var error = Assert.Throws<DependencyResolveException>(() => { container.Resolve<SimpleService>(); });
            
            Assert.That(error.Message, Does.Contain("Service is not registered"));
        }

        [Test]
        public void ItThrowsAnErrorIfTheRegisteredServiceIsAnAbstractClass()
        {
            var error = Assert.Throws<DependencyRegisterException>(() => { container.Register<SimpleBaseClass>(); });
            Assert.That(error.Message, Does.Contain("Service is abstract"));
        }

        [Test]
        public void ItThrowsAnErrorIfTheRegisteredServiceIsAnInterface()
        {
            var error = Assert.Throws<DependencyRegisterException>(() => { container.Register<ISimpleInterface>(); });
            Assert.That(error.Message, Does.Contain("Service is abstract"));
        }

        [Test]
        public void ItThrowsAnErrorIfTheRegisteredServiceIsAnEnum()
        {
            var error = Assert.Throws<DependencyRegisterException>(() => { container.Register<SimpleEnum>(); });
            Assert.That(error.Message, Does.Contain("Service is an enum"));
        }
        
        [Test]
        public void ItCanResolveAServiceByGivingTheTypeAsParameter()
        {
            container.Register<SimpleService>();

            var service = container.Resolve(typeof(SimpleService));
            Assert.IsInstanceOf<SimpleService>(service);
        }
    }
}