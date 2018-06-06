using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class DocumentFactor
    {
        private int _docId;
        private bool _keyword;
        private bool _heading;
        private bool _paragraph;

        public bool Paragraph
        {
            get { return _paragraph; }
            set { _paragraph = value; }
        }

        public bool Heading
        {
            get { return _heading; }
            set { _heading = value; }
        }

        public bool Keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }

        public int DocId
        {
            get { return _docId; }
            set { _docId = value; }
        }

    }
}
