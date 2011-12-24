using System;
using LinkInspector.Objects;
using ManyConsole;
using NDesk.Options;

namespace LinkInspector.Commands
{
    public class ParseUrl : ConsoleCommand
    {
        private string url;
        private string number;
        private string outputFileFormat;
        private string htmlTemplate;
        private bool  errorsOnly;

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
                Console.WriteLine("Can't create Uri from the string specified: '{0}'", url);
                return -1;
            }

            int count = -1;

            if (!string.IsNullOrEmpty(number) && (!Int32.TryParse(number, out count) || count < 1))
            {
                Console.WriteLine("Input argument '{0}' is not an integer value and must be greater than 0.", number);
                return -1;
            }

            if (!string.IsNullOrEmpty(outputFileFormat) && fileFormat == Report.OutputFileFormat.none)
            {
                Console.WriteLine("Unsupported file format {0}", outputFileFormat);
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
