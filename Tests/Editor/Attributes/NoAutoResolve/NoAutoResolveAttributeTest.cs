using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Attributes.NoAutoResolve
{
    public class NoAutoResolveAttributeTest : DucktionTest
    {
        protected override DucktionTestConfig Configure() => new(
            enableAutoResolve: true
        );

        [Test]
        public void ItCanProtectServicesFromBeingAutoResolved()
        {
            var failed = false;
            try
            {
                container.Resolve<ServiceWithNoAutoResolve>();
            }
            catch (DependencyResolveException)
            {
                failed = true;
            }

            Assert.IsTrue(failed, "Service could be autoresolved even though it should not!");
        }

        [Test]
        public void ItStillAllowsProtectedServicesToBeRegisteredManually()
        {
            container.Register<ServiceWithNoAutoResolve>();

            var service = container.Resolve<ServiceWithNoAutoResolve>();
            Assert.IsNotNull(service);
        }
    }
}
