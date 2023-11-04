using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor
{
    public abstract class DucktionTest
    {
        protected DiContainer container;

        [SetUp]
        public void SetUp()
        {
            var config = Configure();
            if (!config.CreateContainer)
            {
                return;
            }

            container = CreateContainer();
        }

        [TearDown]
        public void TearDown()
        {
            Ducktion.Clear();
        }

        protected static DiContainer CreateContainer() => new GameObject("Container").AddComponent<DiContainer>();

        protected virtual DucktionTestConfig Configure() => new(
            createContainer: true
        );
    }
}