using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class ParameterBindingTest : DucktionTest
    {
        [Test]
        public void ItCanBindSpecificParameters()
        {
            container.Register<ScalarService>().SetParameter("value", 24);

            var service = container.Resolve<ScalarService>();
            Assert.AreEqual(24, service.Value);
        }

        [Test]
        public void ItThrowsAnExceptionIfTheTypeIsWrong()
        {
            container.Register<ScalarService>().SetParameter("value", "24");
            var failed = false;

            try
            {
               container.Resolve<ScalarService>();
            }
            catch (DependencyResolveException)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }
    }
}
