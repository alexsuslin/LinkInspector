using LinkInspector.Properties;
using ManyConsole;
using NLog;

namespace LinkInspector.Commands
{
    internal sealed class GetHelp : ConsoleCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GetHelp()
        {
            OneLineDescription = Resources.GetHelpCommandDescription;
            Command = "-h";
        }

        public override int Run()
        {
            logger.Info(Resources.GetHelpRunUsage);
            return 0;
        }
    }
}
