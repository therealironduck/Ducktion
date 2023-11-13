using System;
using System.Collections.Generic;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Editor.Tests.Editor.Fakes
{
    public class FakeLogger : DucktionLogger
    {
        public readonly List<Tuple<LogLevel, string>> Messages = new();
        
        public override void Log(LogLevel level, string message)
        {
            Messages.Add(new Tuple<LogLevel, string>(level, message));
        }

        public void AssertHasMessage(LogLevel level, string message)
        {
            foreach (var (logLevel, logMessage) in Messages)
            {
                if (logLevel == level && logMessage == message)
                {
                    return;
                }
            }
            
            throw new Exception($"Expected to find a log message with level {level} and message {message}");
        }
        
        public void AssertHasNoMessage(LogLevel level, string message)
        {
            foreach (var (logLevel, logMessage) in Messages)
            {
                if (logLevel == level && logMessage == message)
                {
                    throw new Exception($"Expected to not find a log message with level {level} and message {message}");
                }
            }
        }
    }
}