using System;
using LinkInspector.Objects;
using ManyConsole;

namespace LinkInspector
{
    sealed class App
    {
        public class cred
        {
            public string username { get; set; }
            public string password { get; set; }

        }

        static void Main(string[] args)
        {
            using (new Timing("Launch and finish"))
            {
                var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(App));
                ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);    
            }
        }
    }
}