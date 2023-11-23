using System;
using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class IdTest : DucktionTest
    {
        [Test]
        public void ItCanRegisterTheSameServiceWithDifferentIds()
        {
            container.Register<ISimpleInterface, SimpleService>("service1");
            container.Register<ISimpleInterface, SecondSimpleService>("service2");

            Assert.That(container.Resolve<ISimpleInterface>("service1"), Is.InstanceOf<SimpleService>());
            Assert.That(container.Resolve<ISimpleInterface>("service2"), Is.InstanceOf<SecondSimpleService>());
        }

        [Test]
        public void ItThrowsAnErrorIfAServiceWithTheSameIdIsRegisteredTwice()
        {
            container.Register<ISimpleInterface, SimpleService>("service1");

            var error = Assert.Throws<DependencyRegisterException>(() =>
            {
                container.Register<ISimpleInterface, SecondSimpleService>("service1");
            });

            Assert.That(error.Message, Does.Contain("Service is already registered"));
        }

        [Test]
        public void ItWorksWithEveryRegisterMethodSyntax()
        {
            container.Register(typeof(ISimpleInterface), typeof(SimpleService), "id1");
            Assert.That(container.Resolve<ISimpleInterface>("id1"), Is.InstanceOf<SimpleService>());

            container.Register(typeof(SimpleService), "id2");
            Assert.That(container.Resolve<SimpleService>("id2"), Is.InstanceOf<SimpleService>());

            container.Register<SimpleService>("id3");
            Assert.That(container.Resolve<SimpleService>("id3"), Is.InstanceOf<SimpleService>());

            container.Register<ISimpleInterface, SimpleService>("id4");
            Assert.That(container.Resolve<ISimpleInterface>("id4"), Is.InstanceOf<SimpleService>());
        }

        [Test]
        public void ItCanOverrideServicesWithIds()
        {
            container.Register<ISimpleInterface, SimpleService>(id: "service123");

            container.Override(typeof(ISimpleInterface), typeof(SecondSimpleService), id: "service123");

            Assert.That(container.Resolve<ISimpleInterface>("service123"), Is.InstanceOf<SecondSimpleService>());
        }

        [Test]
        public void ItWorksWithEveryOverrideMethodSyntax()
        {
            container.Register<ISimpleInterface, SimpleService>(id: "id1");
            container.Override(typeof(ISimpleInterface), typeof(SecondSimpleService), id: "id1");
            Assert.That(container.Resolve<ISimpleInterface>("id1"), Is.InstanceOf<SecondSimpleService>());

            container.Register<ISimpleInterface, SimpleService>(id: "id2");
            container.Override<ISimpleInterface, SecondSimpleService>(id: "id2");
            Assert.That(container.Resolve<ISimpleInterface>("id2"), Is.InstanceOf<SecondSimpleService>());

            var specific1 = new SimpleService();
            container.Register<ISimpleInterface, SimpleService>(id: "id3");
            container.Override(typeof(ISimpleInterface), id: "id3").SetInstance(specific1);
            Assert.That(container.Resolve<ISimpleInterface>("id3"), Is.SameAs(specific1));
            
            var specific2 = new SimpleService();
            container.Register<ISimpleInterface, SimpleService>(id: "id4");
            container.Override<ISimpleInterface>(id: "id4").SetInstance(specific2);
            Assert.That(container.Resolve<ISimpleInterface>("id4"), Is.SameAs(specific2));
        }
    }
}