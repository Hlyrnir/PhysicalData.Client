using System;
using System.Collections.Specialized;
using System.Web;

namespace PhysicalData.Presentation.Extension
{
    public static class HttpExtension
    {
        public static Uri AddQueryParameter(this Uri httpUri, string sKey, object? oValue)
        {
            NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(httpUri.Query);

            if (oValue is not null)
                httpValueCollection.Set(sKey, oValue.ToString());

            UriBuilder uriBuilder = new UriBuilder(httpUri);
            uriBuilder.Query = httpValueCollection.ToString();

            return uriBuilder.Uri;
        }
    }
}
