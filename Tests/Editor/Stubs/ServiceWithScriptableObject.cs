namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Stubs
{
    public class ServiceWithScriptableObject
    {
        public SimpleScriptableObject someObject;

        public ServiceWithScriptableObject(SimpleScriptableObject so)
        {
            someObject = so;
        }
    }
}
