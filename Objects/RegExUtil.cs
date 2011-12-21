using System.Text.RegularExpressions;

namespace LinkInspector.Objects
{
    public static class RegExUtil
    {
        public const string UrlExtractor = @"(?: href\s*=)(?:[\s""']*)(?!#|mailto|location.|javascript|.*css|.*this\.)(?<url>.*?)(?:[\s>""'])";
        //src extractor = Regex(@"(?:src\s*=)(?:[\s""']*)(?<url>.*?)(?:[\s>""'])", RegexOptions.IgnoreCase);
        
        public static Match GetMatchRegEx(string text)
        {
            return new Regex(UrlExtractor, RegexOptions.IgnoreCase).Match(text);
        }
    }
}