using NUnit.Framework;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Exceptions
{
    public class DucktionExceptionTest
    {
        [Test]
        public void ItSetsTheMessage()
        {
            var exception = new DucktionException("Test");
            Assert.AreEqual("Test", exception.Message);
        }
    }
}