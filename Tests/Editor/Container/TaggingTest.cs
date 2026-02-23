using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class TaggingTest : DucktionTest
    {
        [Test]
        public void ItCanTagServices()
        {
            container.Register<SimpleService>().WithTag("example");
            container.Register<AnotherService>().WithTag("example");
            container.Register<ServiceWithLogger>().WithTag("another_tag");

            var tagged = container.GetTagged("example");
            Assert.AreEqual(2, tagged.Count);

            using var enumerator = tagged.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());

            var service1 = enumerator.Current;
            Assert.NotNull(service1);
            Assert.AreEqual(typeof(SimpleService), service1.ServiceType);

            Assert.IsTrue(enumerator.MoveNext());
            var service2 = enumerator.Current;
            Assert.NotNull(service2);
            Assert.AreEqual(typeof(AnotherService), service2.ServiceType);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ItCanIterateThroughResolvedServices()
        {
            container.Register<SimpleService>().WithTag("example");
            container.Register<AnotherService>().WithTag("example");

            var tagged = container.GetTagged("example");
            Assert.AreEqual(2, tagged.Count);

            var enumerator = tagged.GetServices();
            Assert.IsTrue(enumerator.MoveNext());

            var service1 = enumerator.Current;
            Assert.IsInstanceOf<SimpleService>(service1);

            Assert.IsTrue(enumerator.MoveNext());
            var service2 = enumerator.Current;
            Assert.IsInstanceOf<AnotherService>(service2);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ItCanFilterTaggedServicesByType()
        {
            container.Register<SimpleService>().WithTag("example");
            container.Register<SimpleExtendingService>().WithTag("example");
            container.Register<AnotherService>().WithTag("example");

            var tagged = container.GetTagged("example");
            Assert.AreEqual(3, tagged.Count);

            var enumerator = tagged.GetServices<SimpleService>();
            Assert.IsTrue(enumerator.MoveNext());

            var service1 = enumerator.Current;
            Assert.IsInstanceOf<SimpleService>(service1);

            Assert.IsTrue(enumerator.MoveNext());
            var service2 = enumerator.Current;
            Assert.IsInstanceOf<SimpleExtendingService>(service2);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void ItCanHaveMultipleTags()
        {
            container.Register<SimpleService>().WithTag("example").AddTag("another_tag");
            container.Register<AnotherService>().WithTags("example", "third_tag");
            container.Register<ServiceWithLogger>().AddTag("another_tag");

            var taggedExample = container.GetTagged("example");
            Assert.AreEqual(2, taggedExample.Count);

            var taggedAnother = container.GetTagged("another_tag");
            Assert.AreEqual(2, taggedAnother.Count);

            var taggedThird = container.GetTagged("third_tag");
            Assert.AreEqual(1, taggedThird.Count);
        }

        [Test]
        public void ItCanRemoveSpecificTags()
        {
            container.Register<SimpleService>().WithTag("example").AddTag("another_tag");
            container.Override<SimpleService>().RemoveTag("another_tag");

            var taggedExample = container.GetTagged("example");
            Assert.AreEqual(1, taggedExample.Count);

            var taggedAnother = container.GetTagged("another_tag");
            Assert.AreEqual(0, taggedAnother.Count);
        }
    }
}
