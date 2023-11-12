using System;
using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor
{
    public class LoggingTest : DucktionTest
    {
        [Test]
        public void ItLogsReinitializationOnTheInfoChannel()
        {
            FakeLogger().AssertHasMessage(LogLevel.Info, "Reinitialized container");
        }

        [Test]
        public void ItLogsRegisteredServicesOnTheDebugChannel()
        {
            var logger = FakeLogger();

            container.Register<SimpleService>();

            logger.AssertHasMessage(
                LogLevel.Debug,
                $"Registered service: {typeof(SimpleService)} => {typeof(SimpleService)}"
            );
        }

        [Test]
        public void ItLogsAnErrorIfTheServiceTryingToRegisterAlreadyExists()
        {
            var logger = FakeLogger();

            container.Register<SimpleService>();

            try
            {
                container.Register<SimpleService>();
            }
            catch (Exception)
            {
                // ignored
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(SimpleService)} is already registered"
            );
        }

        [Test]
        public void ItLogsOverriddenServicesOnTheDebugChannel()
        {
            var logger = FakeLogger();

            container.Register<SimpleService>();
            container.Override<SimpleService, SimpleService>();

            logger.AssertHasMessage(
                LogLevel.Debug,
                $"Overridden service: {typeof(SimpleService)} => {typeof(SimpleService)}"
            );
        }

        [Test]
        public void ItLogsAnErrorIfTheServiceTryingToOverrideDoesntExist()
        {
            var logger = FakeLogger();

            try
            {
                container.Override<SimpleService, SimpleService>();
            }
            catch (Exception)
            {
                // ignored
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(SimpleService)} is not registered"
            );
        }

        [Test]
        public void ItLogsClearsOnTheInfoChannel()
        {
            var logger = FakeLogger();

            container.Clear();

            logger.AssertHasMessage(
                LogLevel.Info,
                "Clearing container"
            );
        }

        [Test]
        public void ItLogsResetsOnTheInfoChannel()
        {
            var logger = FakeLogger();

            container.ResetSingletons();

            logger.AssertHasMessage(
                LogLevel.Info,
                "Resetting container"
            );
        }

        [Test]
        public void ItLogsResolvedServicesOnTheDebugChannel()
        {
            var logger = FakeLogger();

            container.Register<ISimpleInterface, SimpleService>();
            container.Resolve<ISimpleInterface>();

            logger.AssertHasMessage(
                LogLevel.Debug,
                $"Resolved service: {typeof(ISimpleInterface)} => {typeof(SimpleService)}"
            );
        }

        [Test]
        public void ItLogsAnErrorIfTheServiceTryingToResolveIsntRegistered()
        {
            var logger = FakeLogger();

            try
            {
                container.Resolve<ISimpleInterface>();
            }
            catch (Exception)
            {
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(ISimpleInterface)} is not registered"
            );
        }

        [Test]
        public void ItLogsAnErrorIfTheServiceHasMultipleConstructors()
        {
            var logger = FakeLogger();

            container.Register<ServiceWithMultipleConstructors>();

            try
            {
                container.Resolve<ServiceWithMultipleConstructors>();
            }
            catch (Exception)
            {
                // ignored
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(ServiceWithMultipleConstructors)} has multiple constructors"
            );
        }
        
        [Test]
        public void ItLogsAnErrorIfACircularDependencyWasFound()
        {
            var logger = FakeLogger();

            container.Register<RecursiveAService>();
            container.Register<RecursiveBService>();

            try
            {
                container.Resolve<RecursiveAService>();
            }
            catch (Exception)
            {
                // ignored
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(RecursiveBService)} has a circular dependency"
            );
        }
        
        [Test]
        public void ItLogsAnErrorIfAnyParameterCantBeResolved()
        {
            var logger = FakeLogger();

            container.Register<ScalarService>();

            try
            {
                container.Resolve<ScalarService>();
            }
            catch (Exception)
            {
                // ignored
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(ScalarService)} cant resolve parameter: value"
            );
        }
        
        [Test]
        public void ItLogsAnErrorIfTheRegisteringServiceIsAbstract()
        {
            var logger = FakeLogger();

            try
            {
                container.Register<SimpleBaseClass>();
            }
            catch (Exception)
            {
                // ignored
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(SimpleBaseClass)} is abstract"
            );
        }
        
        [Test]
        public void ItLogsAnErrorIfTheRegisteringServiceIsAnEnum()
        {
            var logger = FakeLogger();

            try
            {
                container.Register<SimpleEnum>();
            }
            catch (Exception)
            {
                // ignored
            }

            logger.AssertHasMessage(
                LogLevel.Error,
                $"Service {typeof(SimpleEnum)} is an enum"
            );
        }
    }
}