using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using LinkInspector.Objects;
using ManyConsole;
using NDesk.Options;
using NLog;

namespace LinkInspector.Commands
{
    public sealed class CheckFile: ConsoleCommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string path;
        private string username;
        private string password;
        private string domain;

        public CheckFile()
        {
            OneLineDescription = "Input file path";
            Command = "-c";

            Options = new OptionSet
                          {
                              {"l|login=", "Login/Username", v => username = v},
                              {"p|password=", "Passoword", v => password = v},
                              {"d|domain=", "Domain", v => domain = v}
                          };
        }

        public override void FinishLoadingArguments(string[] remainingArguments)
        {
            VerifyNumberOfArguments(remainingArguments, 1);
            path = remainingArguments[0];
        }

        public override int Run()
        {
            if(!File.Exists(path))
            {
                Logger.Error("Specified file was not found. Pleae check file path.");
                return -1;
            }

            if (string.IsNullOrEmpty(username) != string.IsNullOrEmpty(password))
            {
                Logger.Error("Username and Password should be both set.");
                return -1;
            }


            Dictionary<string,int> dictionary = new Dictionary<string,int>();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (TextReader tr = new StreamReader(fs))
                {
                    string str;
                    while((str = tr.ReadLine()) != null)
                    {
                        //todo: add validation
                        string key = str.Split('=')[0];
                        int value = Convert.ToInt32(str.Split('=')[1]);
                        if(!dictionary.ContainsKey(key))
                            dictionary.Add(key,value);
                    }
                }
            }
            WebSpiderOptions options = new WebSpiderOptions
            {
                Username = username,
                Password = password,
                Domain = domain
            };
            foreach (KeyValuePair<string, int> pair in dictionary)
            {
                string url = pair.Key.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                  pair.Key.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                      ? pair.Key
                      : string.Format(CultureInfo.InvariantCulture, "http://{0}", pair.Key);
                WebPageState state = new WebPageState(new Uri(url));
                options.WebPageProcessor.Process(state, false);
                bool isOk = pair.Value == (int) state.StatusCode;
                if (isOk)
                    Logger.Info(state + " > Desired: {0} Actual: {1} - Pass", pair.Value, (int) state.StatusCode);
                else
                    Logger.Warn(state + " > Desired: {0} Actual: {1} - Fail", pair.Value, (int) state.StatusCode);
            }

            
            return 0;
        }
    }
}
