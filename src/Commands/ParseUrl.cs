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
        private string username;
        private string password;
        private string domain;
        private string threads;
        private bool  errorsOnly;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        private Report.OutputFileFormat FileFormat
        {
            get
            {
                Report.OutputFileFormat format;
                if (!Enum.TryParse(outputFileFormat, true, out format))
                    format = Report.OutputFileFormat.None;
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
                              {"e|errors", "Do not show successfully parsed links", v => errorsOnly = v!= null},
                              {"l|login=", "Login/Username", v => username = v},
                              {"p|password=", "Passoword", v => password = v},
                              {"d|domain=", "Domain", v => domain = v},
                              {"th|threads=", "Domain", v => threads = v}
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
            
            url = url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || 
                  url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                      ? url
                      : string.Format(CultureInfo.InvariantCulture, "http://{0}", url);
            
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                Logger.Error(CultureInfo.CurrentUICulture, Resources.ParseUrlRunCantCreateUriError);
                return -1;
            }

            int count = -1;

            if (!string.IsNullOrEmpty(number) && (!Int32.TryParse(number, out count) || count < 1))
            {
                Logger.Error(Resources.ParseUrlRunNotIntegerError, number);
                return -1;
            }

            if (!string.IsNullOrEmpty(outputFileFormat) && FileFormat == Report.OutputFileFormat.None)
            {
                Logger.Error(Resources.ParseUrlRunUnsupportedFormatError, outputFileFormat);
                return -1;
            }

            if(string.IsNullOrEmpty(username) != string.IsNullOrEmpty(password))
            {
                Logger.Error("Username and Password should be both set.");
                return -1;
            }

            int threadsNumber = 1;
            if (!string.IsNullOrEmpty(threads) && (!Int32.TryParse(threads, out threadsNumber) || threadsNumber < 1))
            {
                Logger.Error(Resources.ParseUrlRunNotIntegerError, number);
                return -1;
            }



            WebSpiderOptions options = new WebSpiderOptions
                                           {
                                               UriProcessedCountMax = count,
                                               ShowSuccessUrls = !errorsOnly,
                                               Username = username,
                                               Password = password,
                                               Domain = domain,
                                               NumberOfThreads = threadsNumber
                                           };
           
            Report report = new WebSpider(uri, options).Execute();
            if (FileFormat != Report.OutputFileFormat.None)
                report.SaveReport(FileFormat, htmlTemplate);
            return 0;
        }
    }
}
