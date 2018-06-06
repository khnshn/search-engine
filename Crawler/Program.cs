using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Alireza Khanshan - A Web Crawler");
            Console.Write("How many pages? ");
            int ALL_PAGES = Convert.ToInt32(Console.ReadLine());
            using (StreamWriter writer =
            new StreamWriter("C:/Crawler/Data/pages.txt"))
            {
                writer.Write(ALL_PAGES);
            }
            Console.WriteLine("crawling...");
            int count = 1;
            List<Link> frontier = Crawler.crawl("http://espn.go.com/nba").Links;
            List<string> blackList = new List<string>();
            List<Document> documents = new List<Document>();
            List<WebPageDocument> webPageDcuments = new List<WebPageDocument>();
            Console.WriteLine(count);
            Console.WriteLine("http://espn.go.com/nba : " + frontier.Count);
            count++;
            while (true)
            {
                if (frontier.Count > 0 && count < ALL_PAGES)
                {
                    //
                    foreach (string href in blackList)
                    {
                        frontier.RemoveAll(item => item.Href != null && item.Href.Equals(href));
                    }
                    //
                    Link crawlMe = frontier[0];
                    if (crawlMe.Href == null)
                    {
                        frontier.Remove(crawlMe);
                    }
                    else if (crawlMe.Href.ToLower().Contains("mailto"))
                    {
                        frontier.Remove(crawlMe);
                    }
                    else if (crawlMe.Href.ToLower().StartsWith("http://espn.go.com/nba"))
                    {
                        frontier.RemoveAll(item => item.Href == crawlMe.Href);
                        blackList.Add(crawlMe.Href);
                        WebPageDocument webPage = Crawler.crawl(crawlMe.Href);
                        List<Link> newLinks = webPage != null ? webPage.Links : null;
                        if (newLinks != null)
                        {
                            Console.Write(count + ". ");
                            Console.WriteLine(webPage.Url);
                            documents.Add(new Document()
                            {
                                Id = count,
                                Description = webPage.Description,
                                Links = newLinks.Count,
                                Title = webPage.Title,
                                Url = webPage.Url
                            });
                            webPage.Id = count;
                            webPageDcuments.Add(webPage);
                            count++;
                            foreach (Link l in newLinks)
                            {
                                frontier.Add(l);
                            }
                        }
                    }
                    else if (crawlMe.Href.ToLower().StartsWith("/nba/"))
                    {
                        frontier.RemoveAll(item => item.Href == crawlMe.Href);
                        blackList.Add(crawlMe.Href);
                        WebPageDocument webPage = Crawler.crawl("http://espn.go.com" + crawlMe.Href);
                        List<Link> newLinks = webPage != null ? webPage.Links : null;
                        if (newLinks != null)
                        {
                            Console.Write(count + ". ");
                            Console.WriteLine(webPage.Url);
                            documents.Add(new Document()
                            {
                                Id = count,
                                Description = webPage.Description,
                                Links = newLinks.Count,
                                Title = webPage.Title,
                                Url = webPage.Url
                            });
                            webPage.Id = count;
                            webPageDcuments.Add(webPage);
                            count++;
                            foreach (Link l in newLinks)
                            {
                                frontier.Add(l);
                            }
                        }
                    }
                    else
                    {
                        frontier.Remove(crawlMe);
                    }
                }
                else
                {
                    break;
                }
            }
            List<InvertedListItem> list = IndexBuilder.build(webPageDcuments);
            using (var db = new LiteDatabase(@"C:/Crawler/Data/crawler.db"))
            {
                var invertedIndex = db.GetCollection<InvertedListItem>("inverted_index");
                foreach (InvertedListItem i in list)
                {
                    invertedIndex.Insert(i);
                }
                var documentDataStore = db.GetCollection<Document>("document_store");
                foreach (Document d in documents)
                {
                    documentDataStore.Insert(d);
                }
            }
            Console.WriteLine("*** done ***");
            Console.ReadKey();
        }
    }
}
