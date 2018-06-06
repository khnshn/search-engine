using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler
{
    class WebPageDocument
    {
        private List<Feature> _paragraphs;
        private List<Feature> _headings;
        private List<Link> _links;
        private string _title;
        private string _url;
        private string _description;
        private int _id;
        private List<Feature> _keywords;

        public List<Feature> Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public List<Link> Links
        {
            get { return _links; }
            set { _links = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public List<Feature> Headings
        {
            get { return _headings; }
            set { _headings = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }


        public List<Feature> Paragraphs
        {
            get { return _paragraphs; }
            set { _paragraphs = value; }
        }

        public List<Feature> buildFeatures(List<string> tokens)
        {
            List<Feature> list = new List<Feature>();
            for (int i = 0; i < tokens.Count; i++)
            {
                Feature feature = list.Find(item => item.Term == tokens[i]);
                if (feature != null)
                {
                    list[list.IndexOf(feature)].Positons.Add(i);
                }
                else
                {
                    list.Add(new Feature() { Term = tokens[i], Positons = new List<int>() { i } });
                }
            }
            return list;
        }
    }
}
