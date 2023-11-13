using TheRealIronDuck.Ducktion.Configurators;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs
{
    public class ExampleConfigurator : IDiConfigurator
    {
        public bool Called = false;

        public void Register(DiContainer container)
        {
            container.Register<ISimpleInterface, SimpleService>();
            container.Register<ScalarService>(new ScalarService(123));
            container.Register<AnotherService>(() => new AnotherService());

            Called = true;
        }
    }
}