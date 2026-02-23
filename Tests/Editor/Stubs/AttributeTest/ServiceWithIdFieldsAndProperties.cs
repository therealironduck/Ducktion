using TheRealIronDuck.Ducktion.Attributes;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithIdFieldsAndProperties
    {
        [Resolve(id: "simple")] public SimpleService Simple;
        [Resolve(id: "another")] public AnotherService AnotherService { get; set; }
    }
}
