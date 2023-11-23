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

            container.Register<ScalarService>().SetCallback(action);
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

            container.Register(typeof(ScalarService)).SetCallback(action);
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

            container.Override<ScalarService>().SetCallback(action);

            var service = container.Resolve<ScalarService>();
            Assert.AreEqual(123, service.Value);
        }

        [Test]
        public void ItCanOverrideCallbacksWithExistingInstance()
        {
            var action = new Func<ScalarService>(() => new ScalarService(123));

            var existing = new ScalarService(42);
            container.Register<ScalarService>().SetInstance(existing);

            container.Override<ScalarService>().SetCallback(action);

            var service = container.Resolve<ScalarService>();
            Assert.AreEqual(123, service.Value);
        }

        [Test]
        public void ItCanOverrideCallbacksWithATypeParameter()
        {
            var action = new Func<ScalarService>(() => new ScalarService(123));

            container.Register<ScalarService>();

            container.Override(typeof(ScalarService)).SetCallback(action);

            var service = container.Resolve<ScalarService>();
            Assert.AreEqual(123, service.Value);
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

            container.Register<ScalarService>().SetCallback(action);

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