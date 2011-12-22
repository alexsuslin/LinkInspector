﻿using System;

namespace LinkInspector.Objects
{
    public class WebSpiderOptions
    {
        #region Properties

        public Uri BaseUri { get; set; }

        public int UriProcessedCountMax { get; set; }

        public bool KeepWebContent { get; set; }

        public IWebPageProcessor WebPageProcessor { get; set; }

        public bool ShowSuccessUrls { get; set; }
        
        #endregion

        #region Constructors

        public WebSpiderOptions()
        {
            BaseUri = null;
            UriProcessedCountMax = -1;
            KeepWebContent = false;
            WebPageProcessor = new WebPageProcessor();
            ShowSuccessUrls = true;
        }

        #endregion
    }
}