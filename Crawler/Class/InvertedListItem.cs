using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class InvertedListItem
    {
        private string _term;
        private List<Posting> _paragraphsPostings;
        private List<Posting> _headingsPostings;
        private List<Posting> _keywordsPostings;

        public List<Posting> KeywordsPostings
        {
            get { return _keywordsPostings; }
            set { _keywordsPostings = value; }
        }


        public string Term
        {
            get { return _term; }
            set { _term = value; }
        }
        public List<Posting> HeadingsPostings
        {
            get { return _headingsPostings; }
            set { _headingsPostings = value; }
        }
        public List<Posting> ParagraphsPostings
        {
            get { return _paragraphsPostings; }
            set { _paragraphsPostings = value; }
        }

    }
}
