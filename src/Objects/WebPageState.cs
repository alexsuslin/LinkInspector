using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace LinkInspector.Objects
{
    public sealed  class WebPageState
    {
        #region Fields

        private readonly Uri uri;
        private HttpStatusCode statusCode;

        #endregion

        #region Properties

        public string Content { get; set; }

        //public bool ProcessSuccessfull { get; set; }

        public bool IsContinueProcess { get; set; }

        public PageStatus Status { get; set; }

        public WebExceptionStatus ExceptionStatus { get; set; }

        public TimeSpan ElapsedTimeSpan { get; set; }

        public Uri Uri
        {
            get { return uri; }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return statusCode;
            }
            set
            {
                Status = GetStatus(value);
                statusCode = value;
            }
        }

        public string StatusCodeDescription { get; set; }

        public List<WebRequestState> Redirects { get; set; }

        public bool IsOk
        {
            get {
                return Status == PageStatus.Success || 
                    ( Status == PageStatus.Redirect && Redirects.Count > 0 && Redirects[Redirects.Count-1].StatusCode.Equals(HttpStatusCode.OK));
            }
        }
        

        #endregion

        #region Enums

        public enum PageStatus
        {
            Pending,
            Success,
            Fail,
            Skip,
            Redirect
        }

        #endregion

        #region Classes

        public class WebRequestState
        {
            public Uri Uri { get; set; }
            public HttpStatusCode StatusCode { get; set; }
        }

        #endregion

        #region Constructors

        public WebPageState(Uri uri)
        {
            this.uri = uri;
            Status = PageStatus.Pending;
            statusCode = 0; // Unknown
            Redirects = new List<WebRequestState>();
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(string.Format(CultureInfo.InvariantCulture, "[{0,5:F2}s] [{1}] : {2} ", ElapsedTimeSpan.TotalSeconds, (int)StatusCode, Uri));
            if(Redirects.Count > 0)
                foreach (WebRequestState redirect in Redirects)
                {
                    sb.AppendFormat("\n{0,19}[{1}] : {2}", string.Empty, (int)redirect.StatusCode, redirect.Uri.AbsoluteUri);
                }
            return sb.ToString();
        }

        public static PageStatus GetStatus(HttpStatusCode httpStatusCode)
        {
            int statusNumber = (int)httpStatusCode;

            if (statusNumber >= 200 && statusNumber < 300)
                return PageStatus.Success;

            if (statusNumber >= 300 && statusNumber < 400)
                return PageStatus.Redirect;

            return PageStatus.Fail;
        }

        #endregion
    }
}