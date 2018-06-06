using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    static class IndexBuilder
    {
        public static List<InvertedListItem> build(List<WebPageDocument> WebPageDocumnets)
        {
            List<InvertedListItem> invertedList = new List<InvertedListItem>();
            foreach (WebPageDocument webDoc in WebPageDocumnets)
            {
                //keywords
                for (int i = 0; i < webDoc.Keywords.Count; i++)
                {
                    int invertedListitemIndex = invertedList.FindIndex(item => item.Term.Equals(webDoc.Keywords[i].Term));
                    if (invertedListitemIndex != -1)
                    {
                        int keywordPostingsIndex = invertedList[invertedListitemIndex].KeywordsPostings.FindIndex(item => item.Id.Equals(webDoc.Id));
                        if (keywordPostingsIndex != -1)
                        {
                            invertedList[invertedListitemIndex].KeywordsPostings[keywordPostingsIndex].Positions = webDoc.Keywords[i].Positons;
                        }
                        else
                        {
                            invertedList[invertedListitemIndex].KeywordsPostings.Add(new Posting() { Id = webDoc.Id, Positions = webDoc.Keywords[i].Positons });
                        }
                    }
                    else
                    {
                        var newItem = new InvertedListItem()
                        {
                            Term = webDoc.Keywords[i].Term,
                            HeadingsPostings = new List<Posting>(),
                            ParagraphsPostings = new List<Posting>(),
                            KeywordsPostings = new List<Posting>()
                        };
                        newItem.KeywordsPostings.Add(new Posting() { Id = webDoc.Id, Positions = webDoc.Keywords[i].Positons });
                        invertedList.Add(newItem);
                    }
                }
                //headings
                for (int i = 0; i < webDoc.Headings.Count; i++)
                {
                    int invertedListitemIndex = invertedList.FindIndex(item => item.Term.Equals(webDoc.Headings[i].Term));
                    if (invertedListitemIndex != -1)
                    {
                        int headingPostingsIndex = invertedList[invertedListitemIndex].HeadingsPostings.FindIndex(item => item.Id.Equals(webDoc.Id));
                        if (headingPostingsIndex != -1)
                        {
                            invertedList[invertedListitemIndex].HeadingsPostings[headingPostingsIndex].Positions = webDoc.Headings[i].Positons;
                        }
                        else
                        {
                            invertedList[invertedListitemIndex].HeadingsPostings.Add(new Posting() { Id = webDoc.Id, Positions = webDoc.Headings[i].Positons });
                        }
                    }
                    else
                    {
                        var newItem = new InvertedListItem()
                        {
                            Term = webDoc.Headings[i].Term,
                            HeadingsPostings = new List<Posting>(),
                            ParagraphsPostings = new List<Posting>(),
                            KeywordsPostings = new List<Posting>()
                        };
                        newItem.HeadingsPostings.Add(new Posting() { Id = webDoc.Id, Positions = webDoc.Headings[i].Positons });
                        invertedList.Add(newItem);
                    }
                }
                //paragraphs
                for (int i = 0; i < webDoc.Paragraphs.Count; i++)
                {
                    int invertedListitemIndex = invertedList.FindIndex(item => item.Term.Equals(webDoc.Paragraphs[i].Term));
                    if (invertedListitemIndex != -1)
                    {
                        int paragraphPostingsIndex = invertedList[invertedListitemIndex].ParagraphsPostings.FindIndex(item => item.Id.Equals(webDoc.Id));
                        if (paragraphPostingsIndex != -1)
                        {
                            invertedList[invertedListitemIndex].ParagraphsPostings[paragraphPostingsIndex].Positions = webDoc.Paragraphs[i].Positons;
                        }
                        else
                        {
                            invertedList[invertedListitemIndex].ParagraphsPostings.Add(new Posting() { Id = webDoc.Id, Positions = webDoc.Paragraphs[i].Positons });
                        }
                    }
                    else
                    {
                        var newItem = new InvertedListItem()
                        {
                            Term = webDoc.Paragraphs[i].Term,
                            HeadingsPostings = new List<Posting>(),
                            ParagraphsPostings = new List<Posting>(),
                            KeywordsPostings = new List<Posting>()
                        };
                        newItem.ParagraphsPostings.Add(new Posting() { Id = webDoc.Id, Positions = webDoc.Paragraphs[i].Positons });
                        invertedList.Add(newItem);
                    }
                }
            }
            return invertedList;
        }
    }
}
