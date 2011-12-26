namespace LinkInspector.Objects
{
    using AppConfig = System.Configuration.ConfigurationManager;
    
    internal static class Config
    {
        #region Properties
      
        internal static string UserAgent = AppConfig.AppSettings["UserAgent"] ?? "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705)";
        internal static string HtmlTemplate = AppConfig.AppSettings["HtmlTemplate"] ?? "Templates\\ReportTemplate.html";
        internal static int ProcessUrlNumber;

        #endregion

        static Config()
        {
            if (!int.TryParse(AppConfig.AppSettings["ProcessUrlNumber"], out ProcessUrlNumber))
                ProcessUrlNumber = -1;
        }
    }
}