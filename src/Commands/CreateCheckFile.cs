using System;
using System.IO;
using System.Net;
using System.Xml;
using LinkInspector.Objects;
using ManyConsole;
using NLog;

namespace LinkInspector.Commands
{
    class CreateCheckFile : ConsoleCommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private string filename;

        public CreateCheckFile()
        {
            Command = "--create";
            OneLineDescription = "Create the checkfile in XML format for future usage.";
            RemainingArgumentsHelpText = "<FILE NAME>";
        }

        public override void FinishLoadingArguments(string[] remainingArguments)
        {
            VerifyNumberOfArguments(remainingArguments, 1);
            filename = remainingArguments[0].EndsWith(".xml") ? remainingArguments[0] : remainingArguments[0] + ".xml";
        }

        public override int Run()
        {
            if(File.Exists(filename))
            {
                Logger.Warn("The file you've entered already exists, please specify another file name.");
                return -1;
            }

            XmlDocument document = new XmlDocument();
            XmlDeclaration dec = document.CreateXmlDeclaration("1.0", null, null);
            document.AppendChild(dec);
            XmlElement root = document.CreateElement("Urls");
            document.AppendChild(root);
            
            do
            {
                Console.WriteLine("Enter the command. Available commands:");
                Console.WriteLine("add : add the entry.");
                Console.WriteLine("end : save the file and quit");
                Console.WriteLine();
                string command = Console.ReadLine();

                switch (command)
                {
                    case "add":
                        XmlElement element = CreateChildElement(document);
                        if (element != null)
                            root.AppendChild(element);
                        break;
                    case "end":
                        SaveFile(document);
                        return 1;
                    default: 
                        Console.WriteLine("The command is not recognized.");
                        break;
                }

            } while (true);
        }

        private XmlElement CreateChildElement(XmlDocument document)
        {
            XmlElement node = document.CreateElement("Url");
            
            Console.WriteLine("Please enter URL");
            Uri url = Helper.NormalizeUrl(Console.ReadLine());
            if(url == null)
            {
                Logger.Warn("The url you've entered is incorrect, breaking...");
                return null;
            }
            node.InnerText = url.AbsoluteUri;
            
            Console.WriteLine("Enter desired status code");
            string desiredStatusCode = Console.ReadLine();
            int placeholderStatusCode;
            if (!int.TryParse(desiredStatusCode, out placeholderStatusCode))
            {
                Logger.Warn("The Status Code you've entered is incorrect, breaking...");
                return null;   
            }
            if(!Enum.IsDefined(typeof(HttpStatusCode), placeholderStatusCode))
            {
                Logger.Warn("The Status Code you've entered is incorrect, breaking...");
                return null;
            }
            node.SetAttribute("DesiredStatus", desiredStatusCode);

            //check if this is redirect
            if(placeholderStatusCode >= 300 && placeholderStatusCode <400)
            {
                Console.WriteLine("Please enter the desired redirect url. Any incorrect value or no-value will dismiss that action.");
                Uri redirect = Helper.NormalizeUrl(Console.ReadLine());
                if (redirect != null)
                    node.SetAttribute("DesiredRedirect", redirect.AbsoluteUri);
            }
            

            //todo check if status code is redirect and propose to enter desired url
            return node;
        }

        private void SaveFile(XmlDocument document)
        {
            document.Save(filename);
            return;
        }
    }
}
