using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor;
using TheRealIronDuck.Ducktion.Tests.Stubs;

namespace TheRealIronDuck.Ducktion.Tests
{
    public class ContainerAutoResolveTest : DucktionTest
    {
        protected override DucktionTestConfig Configure() => new(
            createContainer: false
        );

        [Test]
        public void ItResolvesAnyDependencyOfAlreadyExistingSceneObjects()
        {
            // When we have a new GameObject with a component that has a dependency
            var gameObject = new UnityEngine.GameObject();
            var component = gameObject.AddComponent<ExampleMonoWithResolve>();
            
            // Assert that the dependencies are not resolved yet
            Assert.IsNull(component.Simple);
            Assert.IsNull(component.Another);
            
            // As soon as we initialize the container, the dependencies should be resolved
            // We are using `IsInvoking` here simply because singleton isn't a method but a property
            var _ = Ducktion.singleton;
            
            // Assert that the dependencies are resolved
            Assert.IsNotNull(component.Simple);
            Assert.IsNotNull(component.Another);
        }
    }
}