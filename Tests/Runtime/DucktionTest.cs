using NUnit.Framework;
using UnityEngine;

namespace TheRealIronDuck.Ducktion.Tests
{
    public abstract class DucktionTest
    {
        protected DiContainer container;
        
        [SetUp]
        public void SetUp()
        {
            container = new GameObject("Container").AddComponent<DiContainer>(); 
        }
    }
}