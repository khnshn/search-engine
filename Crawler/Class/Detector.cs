using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    static class Detector
    {
        public static List<Link> linkFinder(string rawHtmlString)
        {
            List<Link> links = new List<Link>();
            MatchCollection matchCollection = Regex.Matches(rawHtmlString, @"(<a.*?>.*?</a>)",
                    RegexOptions.Singleline);
            foreach (Match m in matchCollection)
            {
                string value = m.Groups[1].Value;
                Link link = new Link();
                Match match = Regex.Match(value, @"href=\""(.*?)\""",
                    RegexOptions.Singleline);
                if (match.Success)
                {
                    link.Href = match.Groups[1].Value;
                }
                link.InnerHml = Regex.Replace(value, @"\s*<.*?>\s*", "",
                    RegexOptions.Singleline);
                links.Add(link);
            }
            return links;
        }

        public static void setWebPageInformation(string rawHtmlString, WebPageDocument webPage)
        {
            Match match1 = Regex.Match(rawHtmlString, @"<meta name=\""description\"" content=\""(.*?)\""", RegexOptions.Singleline);
            if (match1.Success)
            {
                webPage.Description = match1.Groups[1].Value;
            }
            Match match2 = Regex.Match(rawHtmlString, @"<meta name=\""keywords\"" content=\""(.*?)\""", RegexOptions.Singleline);
            if (match2.Success)
            {
                webPage.Keywords = webPage.buildFeatures(Tokenizer.tokenize(match2.Groups[1].Value));
            }
            Match match3 = Regex.Match(rawHtmlString, @"<title>(.*?)</title>", RegexOptions.Singleline);
            if (match3.Success)
            {
                webPage.Title = match3.Groups[1].Value;
            }
            MatchCollection matchCollection = Regex.Matches(rawHtmlString, @"<h\d.*?>(.*?)</h1>");
            webPage.Headings = new List<Feature>();
            foreach (Match m in matchCollection)
            {
                string pureText = Regex.Replace(m.Groups[1].Value, "<.*?>", String.Empty);
                List<string> tokens = Tokenizer.tokenize(pureText);
                webPage.Headings= webPage.buildFeatures(tokens);
            }
            MatchCollection matchCollection2 = Regex.Matches(rawHtmlString, @"<p.*?>(.*?)</p>",RegexOptions.Multiline);
            webPage.Paragraphs = new List<Feature>();
            foreach (Match m in matchCollection2)
            {
                string pureText = Regex.Replace(m.Groups[1].Value, "<.*?>", String.Empty);
                List<string> tokens = Tokenizer.tokenize(pureText);
                webPage.Paragraphs = webPage.buildFeatures(tokens);
            }
        }
    }
}
