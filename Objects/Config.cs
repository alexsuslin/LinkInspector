using AppConfig = System.Configuration.ConfigurationManager;

namespace LinkInspector.Objects
{
    public static class Config
    {
        #region Properties

        public static string UserAgent { get; private set; }
        private const string UserAgentDefault = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705)";

        public static string HtmlTemplate { get; private set; }
        private const string HtmlTemplateDefault = "Templates\\ReportTemplate.html";

        #endregion

        #region Constructors

        static Config()
        {
            UserAgent = AppConfig.AppSettings["UserAgent"] ?? UserAgentDefault;
            HtmlTemplate = AppConfig.AppSettings["HtmlTemplate"] ?? HtmlTemplateDefault;
        }

        #endregion
    }
}