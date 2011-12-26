using System;
using System.Globalization;
using LinkInspector.Objects;
using LinkInspector.Properties;
using ManyConsole;
using NDesk.Options;
using NLog;

namespace LinkInspector.Commands
{
    internal sealed class ParseUrl : ConsoleCommand
    {
        private string url;
        private string number;
        private string outputFileFormat;
        private string htmlTemplate;
        private bool  errorsOnly;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        private Report.OutputFileFormat fileFormat
        {
            get
            {
                Report.OutputFileFormat format;
                if (!Enum.TryParse(outputFileFormat, true, out format))
                    format = Report.OutputFileFormat.none;
                return format;
            }
        }



        public ParseUrl()
        {
            Command = "-u";
            OneLineDescription = "Specify the Url to inspect for broken links.";
            RemainingArgumentsHelpText = "http://www.example.com";
            Options = new OptionSet
                          {
                              {"n|number=", "Number of links to check", v => number = v},
                              {"ff|file-format=", "Result fileFormat format [txt|html]", v => outputFileFormat = v},
                              {"t|template=", "Path to file template", v => htmlTemplate = v},
                              {"e|errors", "Do not show successfully parsed links", v => errorsOnly = v!= null}
                          };
        }


        public override void FinishLoadingArguments(string[] remainingArguments)
        {
            VerifyNumberOfArguments(remainingArguments, 1);
            url = remainingArguments[0];
        }

        public override int Run()
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                logger.Error(CultureInfo.InvariantCulture, Resources.ParseUrlRunCantCreateUriError);
                return -1;
            }

            int count = -1;

            if (!string.IsNullOrEmpty(number) && (!Int32.TryParse(number, out count) || count < 1))
            {
                logger.Error(Resources.ParseUrlRunNotIntegerError, number);
                return -1;
            }

            if (!string.IsNullOrEmpty(outputFileFormat) && fileFormat == Report.OutputFileFormat.none)
            {
                logger.Error(Resources.ParseUrlRunUnsupportedFormatError, outputFileFormat);
                return -1;
            }

            WebSpiderOptions options = new WebSpiderOptions {UriProcessedCountMax = count, ShowSuccessUrls = !errorsOnly};
           
            Report report = new WebSpider(uri, options).Execute();
            if (fileFormat != Report.OutputFileFormat.none)
                report.SaveReport(fileFormat, htmlTemplate);
            return 0;
        }
    }
}
