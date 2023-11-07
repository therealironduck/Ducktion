using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Fakes;
using TheRealIronDuck.Ducktion.Logging;
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

            container = CreateContainer(config);
        }

        [TearDown]
        public void TearDown()
        {
            Ducktion.Clear();
        }

        protected static DiContainer CreateContainer(DucktionTestConfig config = default)
        {
            var container = new GameObject("Container").AddComponent<DiContainer>();
            container.Configure(
                newLevel: config.LogLevel,
                newEnableAutoResolve: config.EnableAutoResolve,
                newAutoResolveSingletonMode: config.AutoResolveSingletonMode
            );

            return container;
        }

        protected virtual DucktionTestConfig Configure() => new(
            createContainer: true,
            logLevel: LogLevel.Disabled
        );

        protected FakeLogger FakeLogger()
        {
            container.Override<DucktionLogger, FakeLogger>();
            container.Reinitialize();

            return container.Resolve<DucktionLogger>() as FakeLogger;
        }
    }
}