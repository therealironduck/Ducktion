using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor
{
    public class DucktionMainTest : DucktionTest
    {
        protected override DucktionTestConfig Configure()
        {
            return new DucktionTestConfig(
                createContainer: false
            );
        }

        [Test]
        public void ItCanInstantiateANewContainerAtRuntime()
        {
            var singleton = Ducktion.singleton;
            singleton.Reinitialize();
            
            Assert.IsNotNull(singleton);
            Assert.IsInstanceOf<DiContainer>(singleton);
        }

        [Test]
        public void ItReturnsTheSameContainerEverytime()
        {
            var container1 = Ducktion.singleton;
            container1.Reinitialize();
            container1.Register<SimpleService>();

            var container2 = Ducktion.singleton;
            var service = container2.Resolve<SimpleService>();

            Assert.IsNotNull(service);
        }
        
        [Test]
        public void ItThrowsAnExceptionIfThereAreTwoContainersAtTheSameTime()
        {
            var container1 = CreateContainer();
            Ducktion.RegisterContainer(container1);
            
            var error = Assert.Throws<DucktionException>(() => CreateContainer());
            Assert.AreEqual(
                "There is already a container in the scene. You can only have one container at a time.",
                error.Message
            );
        }
    }
}