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

            container.Register(typeof(ISimpleInterface), new SimpleService(), "id5");
            Assert.That(container.Resolve<ISimpleInterface>("id5"), Is.InstanceOf<SimpleService>());

            container.Register<ISimpleInterface>(new SimpleService(), "id6");
            Assert.That(container.Resolve<ISimpleInterface>("id6"), Is.InstanceOf<SimpleService>());

            container.Register(typeof(ISimpleInterface), new Func<ISimpleInterface>(() => new SimpleService()), "id7");
            Assert.That(container.Resolve<ISimpleInterface>("id7"), Is.InstanceOf<SimpleService>());
            
            container.Register<ISimpleInterface>(new Func<ISimpleInterface>(() => new SimpleService()), "id8");
            Assert.That(container.Resolve<ISimpleInterface>("id8"), Is.InstanceOf<SimpleService>());
        }
    }
}