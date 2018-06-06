using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crawler;
using LiteDB;
using System.IO;

namespace SearchEngine
{
    public partial class Results : System.Web.UI.Page
    {
        int resultsCount;
        protected string result;
        protected string keyword;

        public string Result
        {
            get { return result; }
        }

        public string Keyword
        {
            get { return keyword; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            resultsCount = 0;
            String query = Request.QueryString["q"];
            if (query != null)
            {
                var watch = Stopwatch.StartNew();
                keyword = query;
                result = "";
                //
                int ALL_PAGES = 0;
                using (StreamReader reader = new StreamReader("C:/Crawler/Data/pages.txt"))
                {
                    ALL_PAGES = Convert.ToInt32(reader.ReadLine()) - 1;
                }
                List<string> tokens = Tokenizer.tokenize(query);
                using (var db = new LiteDatabase(@"C:/Crawler/Data/crawler.db"))
                {
                    //level 1 : create list of documents [SEARCH]
                    var invertedIndex = db.GetCollection<InvertedListItem>("inverted_index");
                    List<List<DocumentFactor>> documentFactors = new List<List<DocumentFactor>>();
                    foreach (string token in tokens)
                    {
                        InvertedListItem resultForToken;
                        try
                        {
                            resultForToken = invertedIndex.Find(x => x.Term.Equals(token)).ToList()[0];
                        }
                        catch
                        {
                            continue;
                        }
                        List<DocumentFactor> occuranceList = new List<DocumentFactor>();
                        for (int i = 0; i < resultForToken.HeadingsPostings.Count; i++)
                        {
                            occuranceList.Add(new DocumentFactor()
                            {
                                DocId = resultForToken.HeadingsPostings[i].Id,
                                Heading = true,
                                Keyword = false,
                                Paragraph = false
                            });
                        }
                        for (int i = 0; i < resultForToken.KeywordsPostings.Count; i++)
                        {
                            int tempIndex = occuranceList.FindIndex(x => x.DocId.Equals(resultForToken.KeywordsPostings[i].Id));
                            if (tempIndex.Equals(-1))
                            {
                                occuranceList.Add(new DocumentFactor()
                                {
                                    DocId = resultForToken.KeywordsPostings[i].Id,
                                    Heading = false,
                                    Keyword = true,
                                    Paragraph = false
                                });
                            }
                            else
                            {
                                occuranceList[tempIndex].Keyword = true;
                            }
                        }
                        for (int i = 0; i < resultForToken.ParagraphsPostings.Count; i++)
                        {
                            int tempIndex = occuranceList.FindIndex(x => x.DocId.Equals(resultForToken.ParagraphsPostings[i].Id));
                            if (tempIndex.Equals(-1))
                            {
                                occuranceList.Add(new DocumentFactor()
                                {
                                    DocId = resultForToken.ParagraphsPostings[i].Id,
                                    Heading = false,
                                    Keyword = false,
                                    Paragraph = true
                                });
                            }
                            else
                            {
                                occuranceList[tempIndex].Paragraph = true;
                            }
                        }
                        documentFactors.Add(occuranceList);
                    }
                    //end of level 1 [SEARCH]

                    //level 2 : create intersection lists [ORGANIZE]
                    List<List<int>> resultDocumentIds = new List<List<int>>();
                    if (documentFactors.Count > 1)
                    {
                        HashSet<int> hashSet = new HashSet<int>(getDocIdListFromDocumentFactors(documentFactors[0]));
                        List<HashSet<int>> hashSetList = new List<HashSet<int>>();
                        for (int i = documentFactors.Count - 1; i >= 0; i--)
                        {
                            hashSetList.Add(new HashSet<int>(getDocIdListFromDocumentFactors(documentFactors[i])));
                        }
                        for (int i = 1; i < documentFactors.Count; i++)
                        {
                            hashSet.IntersectWith(getDocIdListFromDocumentFactors(documentFactors[i]));
                            hashSetList.Add(hashSet);
                        }
                        //level 3: removing duplicate documents [CLEANING]
                        List<HashSet<int>> finalHashSetList = new List<HashSet<int>>();
                        List<int> blackList = new List<int>();
                        for (int i = hashSetList.Count - 1; i >= 0; i--)
                        {
                            HashSet<int> temp = new HashSet<int>();
                            foreach (int docId in hashSetList[i])
                            {
                                if (blackList.FindIndex(x => x.Equals(docId)) == -1)
                                {
                                    temp.Add(docId);
                                    blackList.Add(docId);
                                }
                            }
                            finalHashSetList.Add(temp);
                        }
                        //end of level 3 [CLEANING]
                        for (int i = finalHashSetList.Count - 1; i >= 0; i--)
                        {
                            resultDocumentIds.Add(finalHashSetList[i].ToList());
                        }
                    }
                    else if (documentFactors.Count == 1)
                    {
                        resultDocumentIds.Add(getDocIdListFromDocumentFactors(documentFactors[0]));
                    }
                    else
                    {
                        resultDocumentIds = null;
                    }
                    //end of level 2 [ORGANIZE]
                    List<DocumentScore> sortedResult = new List<DocumentScore>();
                    var document_store = db.GetCollection<Document>("document_store");
                    if (resultDocumentIds != null)
                    {
                        //level 4 [RANKING]
                        List<DocumentScore> finalResult = new List<DocumentScore>();
                        int tokensCount = tokens.Count;
                        Document document;
                        for (int i = 0; i < resultDocumentIds.Count; i++)
                        {
                            for (int j = 0; j < resultDocumentIds[i].Count; j++)
                            {
                                document = document_store.Find(x => x.Id == resultDocumentIds[i][j]).ToList()[0];
                                double score = 0;
                                if (i < tokensCount)
                                {
                                    //single token set
                                    DocumentFactor scoreHelper = documentFactors[tokensCount - 1 - i].Find(x => x.DocId == document.Id);
                                    score = (double)document.Links / (ALL_PAGES - 1);//degree centrality
                                    score += (double)((double)document.Links * .5) / (ALL_PAGES - 1);//degree prestige
                                    score +=
                                        ((double)((scoreHelper.Keyword ? 5 : 0) + (scoreHelper.Heading ? 3 : 0) + (scoreHelper.Paragraph ? 1 : 0)) / 9);
                                    score += i;
                                }
                                else
                                {
                                    //intersection set
                                    int boundary = i - (tokensCount - 1);
                                    double tempScore = 0;
                                    for (int k = 0; k < boundary + 1; k++)
                                    {
                                        DocumentFactor scoreHelper = documentFactors[k].Find(x => x.DocId == document.Id);
                                        tempScore +=
                                            ((double)((scoreHelper.Keyword ? 5 : 0) + (scoreHelper.Heading ? 3 : 0) + (scoreHelper.Paragraph ? 1 : 0)) / 9);
                                    }
                                    tempScore = (double)tempScore / (boundary + 1);
                                    score += tempScore;
                                    score += (double)document.Links / (ALL_PAGES - 1);//degree centrality
                                    score += (double)((double)document.Links * .65) / (ALL_PAGES - 1);//degree prestige
                                    score += i;
                                }
                                string theUrl = document.Url.ToLower();
                                foreach (string t in tokens)
                                {
                                    if (theUrl.Contains(t))
                                    {
                                        score += ((double)7 / tokensCount);
                                    }
                                }
                                if (finalResult.FindIndex(x => x.Score == score) == -1)
                                {
                                    finalResult.Add(new DocumentScore()
                                    {
                                        DocId = document.Id,
                                        Score = score
                                    });
                                }
                            }
                        }
                        sortedResult = finalResult.OrderByDescending(x => x.Score).ToList();
                    }
                    //end of level 4 [RANKING]
                    watch.Stop();
                    //level 5 [REPRESENTATION]
                    if (sortedResult.Count != 0)
                    {
                        resultsCount = sortedResult.Count;
                        foreach (DocumentScore d in sortedResult)
                        {
                            Document document = document_store.Find(x => x.Id == d.DocId).ToList()[0];
                            result += "<a href=\"" + document.Url + "\" target=\"_blank\">" + document.Title + "</a><br/>";
                            result += document.Description + "<br/>";
                            result += "<span>" + document.Url + "</span><br/>";
                            result += "<h2>" + d.Score + "</h2>";
                            result += "<br/><br/>";
                        }
                    }
                    else
                    {
                        result = "no results";
                    }
                    //end of level 5 [REPRESENTATION]
                }
                //
                var elplasedMs = watch.ElapsedMilliseconds;
                LabelTime.Text = resultsCount + " results in " + ((double)elplasedMs / 1000) + " seconds";
            }
            else
            {
                Response.Redirect("/Default.aspx");
            }
        }

        private List<int> getDocIdListFromDocumentFactors(List<DocumentFactor> documentFactors)
        {
            List<int> result = new List<int>();
            foreach (DocumentFactor df in documentFactors)
            {
                result.Add(df.DocId);
            }
            return result;
        }
    }
}