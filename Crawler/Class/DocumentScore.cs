using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class DocumentScore
    {
        private int _docId;
        private double _score;

        public double Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public int DocId
        {
            get { return _docId; }
            set { _docId = value; }
        }

    }
}
