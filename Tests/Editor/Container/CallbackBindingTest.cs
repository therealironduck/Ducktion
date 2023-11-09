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
        
        // TEST Type as parameter
        // TEST Overriding callbacks without existing instance
        // TEST Overriding callbacks with existing instance
        // TEST Abstract service (Interface)
        // TEST That it stores the result as a singleton
    }
}