using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    static class Crawler
    {
        public static WebPageDocument crawl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = "A .NET Web Crawler";
            try
            {
                WebPageDocument webPage = new WebPageDocument();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string rawHtmlString = reader.ReadToEnd();
                webPage.Links = Detector.linkFinder(rawHtmlString);
                Detector.setWebPageInformation(rawHtmlString, webPage);
                webPage.Url = url;
                return webPage;
            }
            catch
            {
                return null;
            }
        }

    }
}
