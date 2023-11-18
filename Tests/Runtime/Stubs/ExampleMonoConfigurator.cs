using TheRealIronDuck.Ducktion.Configurators;
using TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs;

namespace TheRealIronDuck.Ducktion.Tests.Stubs
{
    public class ExampleMonoConfigurator : MonoDiConfigurator
    {
        public bool called = false;

        public override void Register(DiContainer container)
        {
            container.Register<ISimpleInterface, SimpleService>();
            container.Register<ScalarService>().SetInstance(new ScalarService(123));
            container.Register<AnotherService>().SetCallback(() => new AnotherService());

            called = true;
        }
    }
}