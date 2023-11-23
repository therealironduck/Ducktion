using System;
using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.Events;
using TheRealIronDuck.Ducktion.Events;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Events
{
    public class SimpleEventBusTest : DucktionTest
    {
        protected override DucktionTestConfig Configure() => new(
            enableEventBus: true
        );

        [Test]
        public void ItCanRegisterAndFireEvents()
        {
            var eventBus = container.Resolve<EventBus>();
            var called = false;
            
            eventBus.Listen<ExampleEvent>(@event =>
            {
                Assert.AreEqual(123, @event.Value);
                called = true;
            });

            eventBus.Fire(new ExampleEvent(123));
            Assert.IsTrue(called);
        }
        
        [Test]
        public void ItCanRegisterMultipleListenersForTheSameEvent()
        {
            var eventBus = container.Resolve<EventBus>();
            var called1 = false;
            var called2 = false;
            
            eventBus.Listen<ExampleEvent>(@event =>
            {
                called1 = true;
            });
            
            eventBus.Listen<ExampleEvent>(@event =>
            {
                called2 = true;
            });

            eventBus.Fire(new ExampleEvent(123));
            Assert.IsTrue(called1);
            Assert.IsTrue(called2);
        }
        
        [Test]
        public void ItLogsAnInfoIfNoListenersWereFound()
        {
            var logger = FakeLogger();
            
            var eventBus = container.Resolve<EventBus>();
            
            eventBus.Fire(new ExampleEvent(123));
            
            logger.AssertHasMessage(
                LogLevel.Info,
                $"No event listeners found for event {typeof(ExampleEvent)}"
            );
        }
        
        [Test]
        public void ItCanForgetAListener()
        {
            var eventBus = container.Resolve<EventBus>();
            var called = false;

            Action<ExampleEvent> action = @event =>
            {
                Assert.AreEqual(123, @event.Value);
                called = true;
            };
            eventBus.Listen<ExampleEvent>(action);

            eventBus.Fire(new ExampleEvent(123));
            Assert.IsTrue(called);

            called = false;
            
            eventBus.Forget<ExampleEvent>(action);
            eventBus.Fire(new ExampleEvent(123));
            Assert.IsFalse(called);
        }
        
        [Test]
        public void ItCanClearAllListenersForASpecificEvent()
        {
            var eventBus = container.Resolve<EventBus>();
            var called1 = false;
            var called2 = false;

            eventBus.Listen<ExampleEvent>(_ => called1 = true);
            eventBus.Listen<ExampleEvent>(_ => called2 = true);
            
            eventBus.Fire(new ExampleEvent(123));
            Assert.IsTrue(called1);
            Assert.IsTrue(called2);

            called1 = false;
            called2 = false;

            eventBus.Clear<ExampleEvent>();
            eventBus.Fire(new ExampleEvent(123));
            Assert.IsFalse(called1);
            Assert.IsFalse(called2);
        }
    }
}