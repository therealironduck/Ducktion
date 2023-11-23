using TheRealIronDuck.Ducktion.Events;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs.Events
{
    public class ExampleEvent : IEvent
    {
        public readonly int Value;
        
        public ExampleEvent(int value)
        {
            Value = value;
        }
    }
}