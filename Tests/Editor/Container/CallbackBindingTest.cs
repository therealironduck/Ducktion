using System;
using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class CallbackBindingTest : DucktionTest
    {
        [Test]
        public void ItCanBindCallbacksWhichGetUsedToResolve()
        {
            var called = false;
            var action = new Func<ScalarService>(() =>
            {
                called = true;

                return new ScalarService(123);
            });

            container.Register<ScalarService>(action);
            Assert.IsFalse(called);

            var service = container.Resolve<ScalarService>();
            Assert.IsTrue(called);
            Assert.AreEqual(123, service.Value);
        }

        [Test]
        public void ItCanBindCallbacksUsingATypeParameter()
        {
            var called = false;
            var action = new Func<ScalarService>(() =>
            {
                called = true;

                return new ScalarService(123);
            });

            container.Register(typeof(ScalarService), action);
            Assert.IsFalse(called);

            var service = container.Resolve<ScalarService>();
            Assert.IsTrue(called);
            Assert.AreEqual(123, service.Value);
        }

        [Test]
        public void ItCanOverrideCallbacksWithoutExistingInstance()
        {
            var action = new Func<ScalarService>(() => new ScalarService(123));

            container.Register<ScalarService>();

            container.Override<ScalarService>(action);

            var service = container.Resolve<ScalarService>();
            Assert.AreEqual(123, service.Value);
        }

        [Test]
        public void ItCanOverrideCallbacksWithExistingInstance()
        {
            var action = new Func<ScalarService>(() => new ScalarService(123));

            var existing = new ScalarService(42);
            container.Register<ScalarService>(existing);

            container.Override<ScalarService>(action);

            var service = container.Resolve<ScalarService>();
            Assert.AreEqual(123, service.Value);
        }

        [Test]
        public void ItCanOverrideCallbacksWithATypeParameter()
        {
            var action = new Func<ScalarService>(() => new ScalarService(123));

            container.Register<ScalarService>();

            container.Override(typeof(ScalarService), action);

            var service = container.Resolve<ScalarService>();
            Assert.AreEqual(123, service.Value);
        }

        [Test]
        public void ItCanRegisterCallbacksWithAbstractServicesOrInterfaces()
        {
            var simpleImplementation = new SimpleService();
            var action = new Func<ISimpleInterface>(() => simpleImplementation);

            container.Register<ISimpleInterface>(action);

            var service = container.Resolve<ISimpleInterface>();
            Assert.AreSame(simpleImplementation, service);
        }

        [Test]
        public void ItStoresTheCallbackResultsAsSingletonByDefault()
        {
            var calledCount = 0;
            var service = new ScalarService(123);

            var action = new Func<ScalarService>(() =>
            {
                calledCount++;
                return service;
            });

            container.Register<ScalarService>(action);

            var service1 = container.Resolve<ScalarService>();
            Assert.AreEqual(1, calledCount);
            Assert.AreEqual(123, service1.Value);

            service1.Value = 456;
            var service2 = container.Resolve<ScalarService>();
            Assert.AreEqual(1, calledCount);
            Assert.AreEqual(456, service2.Value);
        }
    }
}