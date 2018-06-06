using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public static class Tokenizer
    {
        public static List<string> tokenize(string text)
        {
            text = text.ToLower();
            text = text.Replace(".", " ");
            text = text.Replace(":", " ");
            text = text.Replace(";", " ");
            text = text.Replace(",", " ");
            text = text.Replace("!", " ");
            text = text.Replace("?", " ");
            text = text.Replace("(", " ");
            text = text.Replace(")", " ");
            text = text.Replace("\"", " ");
            text = text.Replace("'", " ");
            text = text.Replace("<", " ");
            text = text.Replace(">", " ");
            text = text.Replace("|", " ");
            text = text.Replace("/", " ");
            text = text.Replace("_", " ");
            text = text.Replace("-", " ");
            text = text.Replace("#", " ");
            text = text.Replace("@", " ");
            text = text.Replace("$", " ");
            text = text.Replace("]", " ");
            text = text.Replace("[", " ");
            text = text.Replace("{", " ");
            text = text.Replace("}", " ");
            text = text.Replace("*", " ");
            string[] tokensArray = text.Split(' ');
            List<string> tokensList = new List<string>();
            for (int i = 0; i < tokensArray.Length; i++)
            {
                string check = tokensArray[i].Trim().ToLower();
                //https://en.wikipedia.org/wiki/Most_common_words_in_English
                if (String.IsNullOrEmpty(check) || String.IsNullOrWhiteSpace(check) ||
                    check.Equals("the") || check.Equals("and") ||
                    check.Equals("a") || check.Equals("above") ||
                    check.Equals("that") || check.Equals("under") ||
                    check.Equals("I") || check.Equals("beneath") ||
                    check.Equals("it") || check.Equals("after") ||
                    check.Equals("not") || check.Equals("over") ||
                    check.Equals("he") || check.Equals("into") ||
                    check.Equals("as") || check.Equals("about") ||
                    check.Equals("you") || check.Equals("up") ||
                    check.Equals("this") || check.Equals("from") ||
                    check.Equals("but") || check.Equals("by") ||
                    check.Equals("his") || check.Equals("at") ||
                    check.Equals("they") || check.Equals("with") ||
                    check.Equals("her") || check.Equals("on") ||
                    check.Equals("she") || check.Equals("for") ||
                    check.Equals("or") || check.Equals("in") ||
                    check.Equals("an") || check.Equals("of") ||
                    check.Equals("will") || check.Equals("to") ||
                    check.Equals("my") || check.Equals("their") ||
                    check.Equals("one") || check.Equals("there") ||
                    check.Equals("all") || check.Equals("would")||
                    check.Equals("am") || check.Equals("is") ||
                    check.Equals("are") || check.Equals("me") ||
                    check.Equals("we") || check.Equals("your") ||
                    check.Length.Equals(1) || check.Length > 15)
                {
                    continue;
                }
                else
                {
                    tokensList.Add(check);
                }
            }
            return tokensList;
        }
    }
}
