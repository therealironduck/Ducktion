using NUnit.Framework;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Exceptions
{
    public class DependencyRegisterExceptionTest
    {
        [Test]
        public void ItSetsTheMessage()
        {
            var exception = new DependencyRegisterException(typeof(Ducktion), "Someone stole my spaghetti!");
            Assert.That(exception.Message, Is.EqualTo(
                $"Service of type {typeof(Ducktion)} could not be registered. Reason: Someone stole my spaghetti!"
            ));
        }

        [Test]
        public void ItExtendsTheDucktionException()
        {
            var exception = new DependencyRegisterException(typeof(Ducktion), "Someone stole my spaghetti!");
            Assert.IsInstanceOf<DucktionException>(exception);
        }
    }
}