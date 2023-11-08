using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
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

        [Test]
        public void ItThrowsAnErrorIfTheServiceIsAlreadyRegistered()
        {
            container.Register<SimpleService>();

            var error = Assert.Throws<DependencyRegisterException>(() => { container.Register<SimpleService>(); });
            Assert.That(
                error.Message,
                Does.Contain("Service is already registered. Use `override` to override the service")
            );

            container.Register<ISimpleInterface, SimpleService>();

            var error2 = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Register<ISimpleInterface, SimpleService>();
            });
            Assert.That(
                error2.Message,
                Does.Contain("Service is already registered. Use `override` to override the service")
            );
        }

        [Test]
        public void ItCanRegisterWithATypeParameter()
        {
            container.Register(typeof(SimpleService));

            var service = container.Resolve<SimpleService>();
            Assert.IsInstanceOf<SimpleService>(service);
        }

        [Test]
        public void ItCanRegisterWithTwoTypeParameters()
        {
            container.Register(typeof(ISimpleInterface), typeof(SimpleService));

            var service = container.Resolve<ISimpleInterface>();
            Assert.IsInstanceOf<SimpleService>(service);
        }

        [Test]
        public void ItThrowsAnErrorIfTheServiceDoesntExtendTheKeyWhenUsingTypesAsParameters()
        {
            var error = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Register(typeof(ISimpleInterface), typeof(AnotherService));
            });

            Assert.That(
                error.Message,
                Does.Contain($"Service {typeof(AnotherService)} does not extend {typeof(ISimpleInterface)}")
            );
        }
    }
}