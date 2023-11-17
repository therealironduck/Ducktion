using NUnit.Framework;
using TheRealIronDuck.Ducktion.Components;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor;
using TheRealIronDuck.Ducktion.Tests.Stubs;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Tests.Components
{
    public class DynamicDependencyResolverTest : DucktionTest
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
        public void ItResolvesEveryResolveAttributeDependencies()
        {
            Ducktion.singleton.IsInvoking();
            
            _gameObject = new GameObject();
            var component = _gameObject.AddComponent<ExampleMonoWithResolve>();
            
            Assert.IsNull(component.Simple);
            Assert.IsNull(component.Another);

            _gameObject.AddComponent<DynamicDependencyResolver>();
            
            Assert.IsNotNull(component.Simple);
            Assert.IsNotNull(component.Another);
        }
    }
}