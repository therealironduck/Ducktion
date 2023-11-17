using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor;
using TheRealIronDuck.Ducktion.Tests.Stubs;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Tests
{
    public class ContainerAutoResolveTest : DucktionTest
    {
        private GameObject _gameObject;
        
        protected override DucktionTestConfig Configure() => new(
            createContainer: false
        );

        [TearDown]
        public override void TearDown()
        {
            Object.DestroyImmediate(_gameObject);
            
            base.TearDown();
        }

        [Test]
        public void ItResolvesAnyDependencyOfAlreadyExistingSceneObjects()
        {
            // When we have a new GameObject with a component that has a dependency
            _gameObject = new UnityEngine.GameObject();
            var component = _gameObject.AddComponent<ExampleMonoWithResolve>();
            
            // Assert that the dependencies are not resolved yet
            Assert.IsNull(component.Simple);
            Assert.IsNull(component.Another);
            
            // As soon as we initialize the container, the dependencies should be resolved
            // We are using `IsInvoking` here simply because singleton isn't a method but a property
            Ducktion.singleton.IsInvoking();
            
            // Assert that the dependencies are resolved
            Assert.IsNotNull(component.Simple);
            Assert.IsNotNull(component.Another);
        }
    }
}