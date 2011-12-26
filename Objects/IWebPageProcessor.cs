namespace LinkInspector.Objects
{
    internal interface IWebPageProcessor
    {
        WebPageContent ContentHandler { get; set; }
        bool Process(WebPageState state);
    }
}