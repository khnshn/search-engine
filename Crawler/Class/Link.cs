using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class Link
    {
        private string _href;

        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }
        private string _innerHtml;

        public string InnerHml
        {
            get { return _innerHtml; }
            set { _innerHtml = value; }
        }
        public override string ToString()
        {
            return InnerHml + " -> " + Href;
        }

        public string toHtmlLink()
        {
            return "<a href=" + Href + ">" + InnerHml + "</a>";
        }
    }
}
