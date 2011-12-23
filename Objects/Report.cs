﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RazorEngine;

namespace LinkInspector.Objects
{
    public class Report
    {
        #region Enums

        public enum ReportFormat
        {
            Head,
            Body,
            Footer
        }

        public enum OutputFileFormat
        {
            txt,
            html,
            none
        }

        #endregion

        #region Properties

        public Uri StartUri { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PagesProcessed { get; set; }
        public int PagesPending { get; set; }
        public List<WebPageState> PageStates { get; set; }

        private TimeSpan ElapsedTime
        {
            get { return new TimeSpan(EndTime.Ticks - StartTime.Ticks); }
        }

        #endregion

        #region Constructors

        public Report()
        {
            PageStates = new List<WebPageState>();
            PagesProcessed = 0;
            PagesPending = 0;
            StartTime = DateTime.Now;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return ToString(ReportFormat.Head)
                .Append(ToString(ReportFormat.Body))
                .Append(ToString(ReportFormat.Footer)).ToString();
        }

        public StringBuilder ToString(ReportFormat reportFormat)
        {
            StringBuilder sb = new StringBuilder();
            switch (reportFormat)
            {
                case ReportFormat.Head:
                    sb.AppendLine("======================================================================================================");
                    sb.AppendLine("Proccess URI: " + StartUri.AbsoluteUri);
                    sb.AppendLine("Start At    : " + StartTime.ToString("yyyy-MM-dd hh:mm:ss"));
                    sb.AppendLine("------------------------------------------------------------------------------------------------------");
                    return sb;
                case ReportFormat.Body:
                    int index = 0;
                    int count = PageStates.Count;
                    foreach (WebPageState state in PageStates)
                    {
                        sb.AppendLine(String.Format("{0,4}/{1,-4}: {2}", ++index, count, state));
                    }
                    return sb;
                case ReportFormat.Footer:
                    sb.AppendLine("------------------------------------------------------------------------------------------------------");
                    sb.AppendLine("Pages Processed: " + PagesProcessed);
                    sb.AppendLine("Pages Pending  : " + PagesPending);
                    sb.AppendLine("End At         : " + EndTime.ToString("yyyy-MM-dd hh:mm:ss"));
                    sb.AppendLine(String.Format("Elasped Time   : {0}h {1}m {2}s {3}ms", ElapsedTime.Hours,
                                                ElapsedTime.Minutes, ElapsedTime.Seconds, ElapsedTime.Milliseconds));
                    sb.AppendLine("======================================================================================================");
                    return sb;
                default:
                    return sb;
            }
        }

        public void SaveReport(OutputFileFormat fileFormat, string htmlTemplatePath = "")
        {
            StringBuilder rb = new StringBuilder();
            switch (fileFormat)
            {
                case OutputFileFormat.txt:
                    rb.Append(this);
                    break;
                case OutputFileFormat.html:
                    string path = !string.IsNullOrEmpty(htmlTemplatePath) ? htmlTemplatePath : Config.HtmlTemplate;
                    string content = GetFileContent(path);
                    rb.Append(Razor.Parse(content, this));
                    break;
                default:
                    return;
            }
            using (StreamWriter outfile = new StreamWriter(string.Format("{0}_{1}.{2}", StartUri.DnsSafeHost, DateTime.Now.ToString("yyy-MM-dd_hh-mm-ss"), fileFormat)))
            {
                outfile.Write(rb.ToString());
            }
        }

        private string GetFileContent(string path)
        {
            string template = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    template = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} file could not be read:", path);
                Console.WriteLine(e.Message);
            }
            return template;
        }

        #endregion
    }
}
