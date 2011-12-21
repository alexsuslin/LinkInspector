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
            state.ProcessSuccessfull = false;


            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(state.Uri);
            request.Method = "GET";
            request.UserAgent = Config.UserAgent;
            WebResponse response = null;

            try
            {
                response = request.GetResponse();

                if (response is HttpWebResponse)
                    state.StatusCode = ((HttpWebResponse) response).StatusCode;
                else if (response is FileWebResponse)
                    state.StatusCode = HttpStatusCode.OK;

                //todo add redirect support

                if (state.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var sr = new StreamReader(response.GetResponseStream());
                    state.Content = sr.ReadToEnd();
                    
                    if (ContentHandler != null)
                        ContentHandler(state);
                }

                state.ProcessSuccessfull = true;
            }
            catch (Exception ex)
            {
                //todo error handling
                if (ex is WebException)
                {
                    WebException webEx = (WebException) ex;
                    state.ExceptionStatus = webEx.Status;
                    if (webEx.Response != null && webEx.Response is HttpWebResponse)
                        state.StatusCode = ((HttpWebResponse) ((WebException) ex).Response).StatusCode;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return state.ProcessSuccessfull;
        }

        #endregion
    }
}