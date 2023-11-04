using NUnit.Framework;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;
using TheRealIronDuck.Ducktion.Exceptions;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Container
{
    public class RecursiveResolveTest : DucktionTest
    {
        [Test]
        public void ItCanResolveAServiceRecursively()
        {
            container.Register<ISimpleInterface, SimpleService>();
            container.Register<ServiceWithDependencies>();

            var service = container.Resolve<ServiceWithDependencies>();
            Assert.IsInstanceOf<ServiceWithDependencies>(service);
            Assert.IsInstanceOf<SimpleService>(service.Simple);

            var simple = container.Resolve<ISimpleInterface>();
            Assert.AreSame(simple, service.Simple);
        }

        [Test]
        public void ItThrowsAnErrorIfTheObjectHasMoreThanOneConstructor()
        {
            container.Register<SimpleService>();
            container.Register<AnotherService>();
            container.Register<ServiceWithMultipleConstructors>();

            var error = Assert.Throws<DependencyResolveException>(
                () => container.Resolve<ServiceWithMultipleConstructors>()
            );
            Assert.That(error.Message, Does.Contain("Service has more than one constructor"));
        }

        [Test]
        public void ItThrowsAnErrorIfTheParametersCantBeResolved()
        {
            container.Register<ServiceWithDependencies>();

            var error = Assert.Throws<DependencyResolveException>(
                () => container.Resolve<ServiceWithDependencies>()
            );
            Assert.That(error.Message, Does.Contain("Parameter `simple` could not be resolved"));
        }
        
        [Test]
        public void ItThrowsAnErrorIfTheParametersCantBeResolvedBecauseOfScalarParameters()
        {
            container.Register<ScalarService>();

            var error = Assert.Throws<DependencyResolveException>(
                () => container.Resolve<ScalarService>()
            );
            Assert.That(error.Message, Does.Contain("Parameter `a` could not be resolved"));
        }

        [Test]
        public void ItThrowsAnErrorIfThereAreCircularDependencies()
        {
            container.Register<RecursiveAService>();
            container.Register<RecursiveBService>();
            container.Register<RecursiveWrapperService>();

            var error = Assert.Throws<DependencyResolveException>(
                () => container.Resolve<RecursiveAService>()
            );
            Assert.That(error.Message, Does.Contain("Parameter `b` could not be resolved"));
            
            var error2 = Assert.Throws<DependencyResolveException>(
                () => container.Resolve<RecursiveBService>()
            );
            Assert.That(error2.Message, Does.Contain("Parameter `a` could not be resolved"));
            
            var error3 = Assert.Throws<DependencyResolveException>(
                () => container.Resolve<RecursiveWrapperService>()
            );
            Assert.That(error3.Message, Does.Contain("Parameter `a` could not be resolved"));
        }
    }
}