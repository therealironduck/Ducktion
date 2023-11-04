using System;

namespace TheRealIronDuck.Ducktion.Exceptions
{
    public class DucktionException : Exception
    {
        public DucktionException(string message) : base(message)
        {
        }
    }
}