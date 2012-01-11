namespace LinkInspector.Objects
{
    internal interface IWebPageProcessor
    {
        WebPageContent ContentHandler { get; set; }
        WebSpiderOptions Options { get; set; }
        bool Process(WebPageState state, bool b = true);
    }
}