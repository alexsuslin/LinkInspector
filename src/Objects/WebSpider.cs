using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;
using LinkInspector.Properties;

namespace LinkInspector.Objects
{
    internal sealed class WebSpider
    {
        #region Fields

        private readonly Queue webPagesPending;
        private readonly Hashtable webPages;
        private readonly WebSpiderOptions spiderOptions;

        #endregion

        #region Properties

        public Uri StartUri { get; set; }

        #endregion

        #region Constructors

        public WebSpider(Uri startUri, WebSpiderOptions options = null)
        {
            StartUri = startUri;
            spiderOptions = options ?? new WebSpiderOptions();

            // In future this could be null and will process cross-site, but for now must exist
            spiderOptions.BaseUri = spiderOptions.BaseUri ?? new Uri(StartUri.GetLeftPart(UriPartial.Authority));

            webPagesPending = new Queue();
            webPages = new Hashtable();
            spiderOptions.WebPageProcessor.ContentHandler += HandleLinks;
        }

        #endregion

        #region Methods

        public Report Execute()
        {
            Report report = new Report {StartUri = StartUri};
            
            Console.WriteLine(report.ToString(Report.ReportFormat.Head));

            AddWebPage(StartUri, StartUri.AbsoluteUri);
            Stopwatch sw = new Stopwatch();
//            try
//            { 
                while (webPagesPending.Count > 0 &&
                       (spiderOptions.UriProcessedCountMax == -1 || report.PagesProcessed < spiderOptions.UriProcessedCountMax))
                {
                    Console.Write(Resources.WebSpiderExecuteProcessedUrlsInfo, report.PagesProcessed, webPagesPending.Count);
                    var state = (WebPageState)webPagesPending.Dequeue();
                    sw.Start();                    
                    spiderOptions.WebPageProcessor.Process(state);                    
                    sw.Stop();                    

                    state.ElapsedTimeSpan = sw.Elapsed;
                    if (spiderOptions.ShowSuccessUrls || !state.IsOk)
                        report.PageStates.Add(state);

//                    if (!spiderOptions.KeepWebContent)
//                        state.Content = null;

                    report.PagesProcessed++;
                    Console.WriteLine(state);
                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Failure while running web spider: " + ex);
//            }

            report.EndTime = DateTime.Now;
            Console.WriteLine(report.ToString(Report.ReportFormat.Footer));
            return report;
        }

        public void HandleLinks(WebPageState state)
        {
            if (state != null && state.IsContinueProcess)
            {
                Match m = RegExUtil.GetMatchRegEx(state.Content);
                do 
                    AddWebPage(state.Uri, m.Groups["url"].ToString()); 
                while ((m = m.NextMatch()).Success);
            }
        }

        private void AddWebPage(Uri baseUri, string newUri)
        {
            // Remove any anchors
            int index = newUri.IndexOf("#", StringComparison.OrdinalIgnoreCase);
            string url = (!string.IsNullOrEmpty(newUri) && index > 0) ? newUri.Substring(0, index) : newUri;

            var uri = new Uri(baseUri, url);

            if (webPages.Contains(uri))
                return;

            var state = new WebPageState(uri)
                            {
                                IsContinueProcess = uri.AbsoluteUri.StartsWith(spiderOptions.BaseUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase)
                            };

            webPagesPending.Enqueue(state);
            webPages.Add(uri, state);

            return;
        }

        #endregion
    }
}