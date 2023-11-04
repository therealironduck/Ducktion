using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class OverrideTest : DucktionTest
    {
        [Test]
        public void ItCanOverrideAnyService()
        {
            container.Register<ISimpleInterface, SimpleService>();

            var service = container.Resolve<ISimpleInterface>();
            Assert.IsInstanceOf<SimpleService>(service);

            container.Override<ISimpleInterface, SecondSimpleService>();

            var secondService = container.Resolve<ISimpleInterface>();
            Assert.IsInstanceOf<SecondSimpleService>(secondService);
        }

        [Test]
        public void ItThrowsAnErrorIfTheOverriddenServiceIsAbstract()
        {
            container.Register<ISimpleInterface, SimpleService>();

            var error = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Override<ISimpleInterface, SimpleBaseClass>();
            });
            Assert.That(error.Message, Does.Contain("Service is abstract"));
        }

        [Test]
        public void ItThrowsAnErrorIfTheOverriddenServiceIsAnInterface()
        {
            container.Register<ISimpleInterface, SimpleService>();

            var error = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Override<ISimpleInterface, ISimpleInterface > ();
            });
            Assert.That(error.Message, Does.Contain("Service is abstract"));
        }
        
        [Test]
        public void ItThrowsAnErrorIfTheOverriddenServiceIsAnEnum()
        {
            container.Register<object, SimpleService>();

            var error = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Override<object, SimpleEnum > ();
            });
            Assert.That(error.Message, Does.Contain("Service is an enum"));
        }
        
        [Test]
        public void ItThrowsAnErrorIfTheServiceWasntRegisteredBefore()
        {
            var error = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Override<ISimpleInterface, SimpleService > ();
            });
            Assert.That(error.Message, Does.Contain("Service is not registered. Use `register` to register the service"));
        }
    }
}