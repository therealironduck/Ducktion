using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Tests
{
    public class DucktionSingletonTest : DucktionTest
    {
        protected override DucktionTestConfig Configure() => new(
            createContainer: false
        );

        [Test]
        public void ItUsesAnAlreadyExistingContainerAsSingleton()
        {
            var existingContainer = CreateContainer();
            existingContainer.Register<SimpleService>();

            var container2 = Ducktion.singleton;
            var service = container2.Resolve<SimpleService>();

            Assert.IsNotNull(service);
        }
        
        [Test]
        public void ItCanClearTheSingleton()
        {
            var existingContainer = Ducktion.singleton;
            existingContainer.Register<SimpleService>();

            Ducktion.Clear();

            var container2 = Ducktion.singleton;
            Assert.AreNotEqual(existingContainer, container2);
        }
    }
}