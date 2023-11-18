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
            
            container.Register(typeof(ISimpleInterface), new Func<ISimpleInterface>(() => new SimpleService()), "id5");
            Assert.That(container.Resolve<ISimpleInterface>("id5"), Is.InstanceOf<SimpleService>());

            container.Register<ISimpleInterface>(new Func<ISimpleInterface>(() => new SimpleService()), "id6");
            Assert.That(container.Resolve<ISimpleInterface>("id6"), Is.InstanceOf<SimpleService>());
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

            var specific = new SimpleService();
            container.Register<ISimpleInterface, SimpleService>(id: "id3");
            container.Override(typeof(ISimpleInterface), () => specific, id: "id3");
            Assert.That(container.Resolve<ISimpleInterface>("id3"), Is.SameAs(specific));
            
            var specific2 = new SimpleService();
            container.Register<ISimpleInterface, SimpleService>(id: "id4");
            container.Override<ISimpleInterface>(() => specific2, id: "id4");
            Assert.That(container.Resolve<ISimpleInterface>("id4"), Is.SameAs(specific2));

            var specific3 = new SimpleService();
            container.Register<ISimpleInterface, SimpleService>(id: "id5");
            container.Override(typeof(ISimpleInterface), id: "id5").SetInstance(specific3);
            Assert.That(container.Resolve<ISimpleInterface>("id5"), Is.SameAs(specific3));
            
            var specific4 = new SimpleService();
            container.Register<ISimpleInterface, SimpleService>(id: "id6");
            container.Override<ISimpleInterface>(id: "id6").SetInstance(specific4);
            Assert.That(container.Resolve<ISimpleInterface>("id6"), Is.SameAs(specific4));
        }
    }
}