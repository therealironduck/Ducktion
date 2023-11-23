namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.AttributeTest
{
    public class ServiceWithIdFieldsAndProperties
    {
        [Attributes.Resolve(id: "simple")] public SimpleService Simple;
        [Attributes.Resolve(id: "another")] public AnotherService AnotherService { get; set; }
    }
}