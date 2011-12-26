using System;
using System.IO;
using System.Net;

namespace LinkInspector.Objects
{
    internal sealed class WebPageProcessor : IWebPageProcessor
    {
        #region Properties

        public WebPageContent ContentHandler { get; set; }

        #endregion

        #region Interface Implementation

        public bool Process(WebPageState state)
        {
            WebResponse response = GetDestinationResponse(state);

            bool isProcessSuccessfull =
                response != null &&
                state.IsOk &&
                ContentHandler != null;

            if (isProcessSuccessfull)
                ContentHandler(state);
            
            return isProcessSuccessfull;
        }

        private WebResponse GetDestinationResponse(WebPageState state, Uri redirect = null)
        {
            Uri requestUri = redirect ?? state.Uri;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "GET";
            request.UserAgent = Config.UserAgent;
            request.AllowAutoRedirect = false;
            

            bool isRedirect = false;

            HttpWebResponse response = null;

            try
            {
                if ((response = request.GetResponse() as HttpWebResponse) == null)
                    return null;
                
                if (redirect == null)
                    state.StatusCode = response.StatusCode;
                else if (state.Redirects.Count > 0)
                {
                    state.Redirects[state.Redirects.Count - 1].StatusCode = response.StatusCode;
                }

                state.StatusCodeDescription = response.StatusDescription;
                
                if (WebPageState.GetStatus(response.StatusCode) == WebPageState.PageStatus.Redirect)
                {
                    isRedirect = true;
                    requestUri = new Uri(response.Headers["Location"]);
                    state.Redirects.Add(new WebPageState.WebRequestState { Uri = requestUri});
                }
                else
                {
                    Stream stream = response.GetResponseStream();
                    if (stream != null)
                        state.Content = new StreamReader(stream).ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                //todo error handling
                state.ExceptionStatus = ex.Status;
                if (ex.Response != null && ex.Response is HttpWebResponse)
                        state.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
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