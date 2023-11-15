using TheRealIronDuck.Ducktion.Attributes;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Tests.Stubs
{
    public class ExampleMonoWithResolve : MonoBehaviour
    {
        [Resolve] private readonly SimpleService _simpleService;
        private AnotherService _anotherService;

        [Resolve]
        private void Inject(AnotherService another)
        {
            _anotherService = another;
        }

        public SimpleService Simple => _simpleService;
        public AnotherService Another => _anotherService;
    }
}