using System;
using System.Net;
using System.Security;

namespace LinkInspector.Objects
{
    internal sealed class WebSpiderOptions
    {
        #region Properties

        public Uri BaseUri { get; set; }

        public int UriProcessedCountMax { get; set; }

        public IWebPageProcessor WebPageProcessor { get; set; }

        public bool ShowSuccessUrls { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }

        public NetworkCredential Credential
        {
            get
            {
                return !(string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password)) ? new NetworkCredential(Username, ToSecureString(Password), Domain) : null;
            }
        }

        private int numberOfThreads;
        public int NumberOfThreads
        {
            get { return numberOfThreads;  }
            set { numberOfThreads = value > UriProcessedCountMax ? UriProcessedCountMax : value; }
        }

        public static SecureString ToSecureString(string str)
        {
            SecureString secure = new SecureString();
            foreach (char ch in str)
                secure.AppendChar(ch);
            return secure;
        }

        #endregion

        #region Constructors

        public WebSpiderOptions()
        {
            // todo: from app.config
            BaseUri = null;
            UriProcessedCountMax = -1;
            WebPageProcessor = new WebPageProcessor(this);
            ShowSuccessUrls = true;
            numberOfThreads = 1;
        }

        #endregion
    }
}
