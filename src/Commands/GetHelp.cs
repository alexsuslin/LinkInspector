using LinkInspector.Properties;
using ManyConsole;
using NLog;

namespace LinkInspector.Commands
{
    internal sealed class GetHelp : ConsoleCommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GetHelp()
        {
            OneLineDescription = Resources.GetHelpCommandDescription;
            Command = "-h";
        }

        public override int Run()
        {
            Logger.Info(Resources.GetHelpRunUsage);
            return 0;
        }
    }
}
