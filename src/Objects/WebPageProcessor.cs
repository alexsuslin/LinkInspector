using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using NLog;

namespace LinkInspector.Objects
{
    internal sealed class WebPageProcessor : IWebPageProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region Properties

        public WebPageContent ContentHandler { get; set; }

        public WebSpiderOptions Options { get; set; }

        #endregion

        public WebPageProcessor(WebSpiderOptions options = null)
        {
            Options = options;
        }

        #region Interface Implementation

        public bool Process(WebPageState state, bool checkRedirects = true)
        {
            WebResponse response = GetDestinationResponse(state, null, checkRedirects);
            bool isProcessSuccessfull =
                response != null &&
                state.IsOk &&
                ContentHandler != null;

            if (isProcessSuccessfull)
                ContentHandler(state);
            
            return isProcessSuccessfull;
        }

        private WebResponse GetDestinationResponse(WebPageState state, Uri redirect = null, bool checkRedirects = true)
        {
            Uri requestUri = redirect ?? state.Uri;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "GET";
            request.UserAgent = Config.UserAgent;
            request.AllowAutoRedirect = false;

            if (Options != null && Options.Credential != null)
                request.Credentials = Options.Credential;

            bool isRedirect = false;

            HttpWebResponse response = null;

            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                response = request.GetResponse() as HttpWebResponse;
                sw.Stop();
                state.ElapsedTimeSpan = sw.Elapsed;

                if (response == null)
                    return null;
                
                if (redirect == null)
                    state.StatusCode = response.StatusCode;
                else if (state.Redirects.Count > 0)
                {
                    state.Redirects[state.Redirects.Count - 1].StatusCode = response.StatusCode;
                }

                state.StatusCodeDescription = response.StatusDescription;
                
                if (WebPageState.GetStatus(response.StatusCode) == WebPageState.PageStatus.Redirect && checkRedirects)
                {
                    isRedirect = true;
                    //todo check for URI format
                    string url = response.Headers["Location"];
                    

                    url = url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                          url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                              ? url
                              : string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Options.BaseUri, url);
                    requestUri = new Uri(url);

                    state.Redirects.Add(new WebPageState.WebRequestState { Uri = requestUri});
                }
                else if (response.ContentType.StartsWith("text", StringComparison.OrdinalIgnoreCase))
                {
                    Stream stream = response.GetResponseStream();
                    if (stream != null)
                        state.Content = new StreamReader(stream).ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Logger.LogException(LogLevel.Fatal, ex.Status.ToString(), ex);
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    state.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    state.StatusCodeDescription = ((HttpWebResponse) ex.Response).StatusDescription;
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return isRedirect && checkRedirects ? GetDestinationResponse(state, requestUri) : response;
        }

        #endregion
    }
}