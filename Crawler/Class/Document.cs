using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class Document
    {
        private int _id;
        private string _url;
        private string _title;
        private string _description;
        private int _links;

        public int Links
        {
            get { return _links; }
            set { _links = value; }
        }


        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

    }
}
