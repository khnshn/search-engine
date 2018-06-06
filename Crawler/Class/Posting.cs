using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class Posting
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private List<int> _positions;

        public List<int> Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }

    }
}
