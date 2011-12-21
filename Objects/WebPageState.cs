using System;
using System.Net;

namespace LinkInspector.Objects
{
    public sealed class WebPageState
    {
        #region Fields

        private readonly Uri uri;
        private HttpStatusCode statusCode;

        #endregion

        #region Properties

        public string Content { get; set; }

        public bool ProcessSuccessfull { get; set; }

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
                int statusNumber = (int)value;
                if (statusNumber >= 200 && statusNumber < 300)
                    Status = PageStatus.Success;
                else if (statusNumber >= 300 && statusNumber < 400)
                    Status = PageStatus.Redirect;
                else
                    Status = PageStatus.Fail;

                statusCode = value;
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

        #region Constructors

        public WebPageState(Uri uri)
        {
            this.uri = uri;
            Status = PageStatus.Pending;
            statusCode = 0; // Unknown
        }

        public WebPageState(string uri)
            : this(new Uri(uri))
        {
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("[{0,5:F2}s] [{1}] : {2} ", ElapsedTimeSpan.TotalSeconds, (int)StatusCode, Uri);
        }

        #endregion
    }
}