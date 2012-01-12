using System;
using System.Globalization;

namespace LinkInspector.Objects
{
    public class Helper
    {
        public static Uri NormalizeUrl(string url, string baseUri = null)
        {
            string returnUrl = url;
            if (!string.IsNullOrEmpty(baseUri) && !returnUrl.StartsWith(baseUri))
                returnUrl = string.Format("{0}/{1}", baseUri, returnUrl);
            if (!returnUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !returnUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) && !returnUrl.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                returnUrl = string.Format(CultureInfo.InvariantCulture, "http://{0}", returnUrl);
            return Uri.IsWellFormedUriString(returnUrl, UriKind.Absolute) ? new Uri(returnUrl, UriKind.Absolute) : null;
        }
    }
}
