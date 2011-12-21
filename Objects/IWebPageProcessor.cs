namespace LinkInspector.Objects
{
    public interface IWebPageProcessor
    {
        WebPageContentDelegate ContentHandler { get; set; }
        bool Process(WebPageState state);
    }
}