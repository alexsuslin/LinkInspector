using System;
using System.IO;
using System.Net;

namespace LinkInspector.Objects
{
    public class WebPageProcessor : IWebPageProcessor
    {
        #region Properties

        public WebPageContentDelegate ContentHandler { get; set; }

        #endregion

        #region Interface Implementation

        public bool Process(WebPageState state)
        {
            WebResponse response = GetDestinationResponse(state);

            if (response == null)
                return false;

//            if (response is HttpWebResponse)
//                state.StatusCode = ((HttpWebResponse)response).StatusCode;
//            else if (response is FileWebResponse)
//                state.StatusCode = HttpStatusCode.OK;

            if (state.StatusCode.Equals(HttpStatusCode.OK))
            {
                if (ContentHandler != null)
                    ContentHandler(state);
            }

            return true;
        }

        private WebResponse GetDestinationResponse(WebPageState state, Uri redirect = null)
        {
            Uri requestUri = redirect ?? state.Uri;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "GET";
            request.UserAgent = Config.UserAgent;
            request.AllowAutoRedirect = false;
            

            bool isRedirect = false;
            
            WebResponse response = null;

            try
            {
                response = request.GetResponse();

                if (redirect == null)
                    state.StatusCode = ((HttpWebResponse)response).StatusCode;
                else if (state.Redirects.Count > 0)
                {
                    state.Redirects[state.Redirects.Count - 1].StatusCode = ((HttpWebResponse) response).StatusCode;
                }
                
                if (response is HttpWebResponse && state.GetStatus(((HttpWebResponse)response).StatusCode) == WebPageState.PageStatus.Redirect)
                {
                    isRedirect = true;
                    requestUri = new Uri(response.Headers["Location"]);
                    state.Redirects.Add(new WebPageState.WebRequestState { Uri = requestUri});
                }
                else if (response is HttpWebResponse)
                {
                    state.Content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                
            }
            catch (Exception ex)
            {
                //todo error handling
                if (ex is WebException)
                {
                    WebException webEx = (WebException)ex;
                    state.ExceptionStatus = webEx.Status;
                    if (webEx.Response != null && webEx.Response is HttpWebResponse)
                        state.StatusCode = ((HttpWebResponse)((WebException)ex).Response).StatusCode;
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return isRedirect ? GetDestinationResponse(state, requestUri) : response;
        }

        #endregion
    }
}