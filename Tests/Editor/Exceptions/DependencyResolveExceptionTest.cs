using NUnit.Framework;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Exceptions
{
    public class DependencyResolveExceptionTest
    {
        [Test]
        public void ItSetsTheMessage()
        {
            var exception = new DependencyResolveException(typeof(Ducktion), "Someone stole my spaghetti!");
            Assert.That(exception.Message, Is.EqualTo(
                $"The service {typeof(Ducktion)} could not be resolved. Reason: Someone stole my spaghetti!"
            ));
        }
        
        [Test]
        public void ItExtendsTheDucktionException()
        {
            var exception = new DependencyResolveException(typeof(Ducktion), "Someone stole my spaghetti!");
            Assert.IsInstanceOf<DucktionException>(exception);
        }
    }
}