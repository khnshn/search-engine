using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class Feature
    {
        private string _term;

        public string Term
        {
            get { return _term; }
            set { _term = value; }
        }
        private List<int> _positions;

        public List<int> Positons
        {
            get { return _positions; }
            set { _positions = value; }
        }
        
    }
}
