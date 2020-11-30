using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LuceneNetDemo.Repository
{
    public class LuceneAnalyze
    {
        #region AnalyzerKey
        public string AnalyzerKeyword(string name,string keyword)
        {
            StringBuilder queryStringBuilder = new StringBuilder();
 
            string[] words = AnalyzerKey(keyword);
            if (words.Length == 1)
            {
                queryStringBuilder.AppendFormat("{0}:{1}* ", name, words[0]);
            }
            else
            {
                StringBuilder fieldQueryStringBuilder = new StringBuilder();
                foreach (string word in words)
                {
                    queryStringBuilder.AppendFormat("{0}:{1} ", name, word);
                }
            }
            return queryStringBuilder.ToString().TrimEnd();
        }

        /// <summary>
        /// 将搜索的keyword分词
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string[] AnalyzerKey(string keyword)
        {
            Analyzer analyzer = new PanGuAnalyzer();
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "title", analyzer);
            Query query = parser.Parse(this.CleanKeyword(keyword));
            if (query is TermQuery)
            {
                Term term = ((TermQuery)query).Term;
                return new string[] { term.Text };
            }
            else if (query is PhraseQuery)
            {
                Term[] term = ((PhraseQuery)query).GetTerms();
                return term.Select(t => t.Text).ToArray();
            }
            else if (query is BooleanQuery)
            {
                BooleanClause[] clauses = ((BooleanQuery)query).GetClauses();
                List<string> analyzerWords = new List<string>();
                foreach (BooleanClause clause in clauses)
                {
                    Query childQuery = clause.Query;
                    if (childQuery is TermQuery)
                    {
                        Term term = ((TermQuery)childQuery).Term;
                        analyzerWords.Add(term.Text);
                    }
                    else if (childQuery is PhraseQuery)
                    {
                        Term[] term = ((PhraseQuery)childQuery).GetTerms();
                        analyzerWords.AddRange(term.Select(t => t.Text));
                    }
                }
                return analyzerWords.ToArray();
            }
            else
            {
                return new string[] { keyword };
            }
        }

        /// <summary>
        /// 清理头尾and or 关键字
        /// 
        /// 为了避免在查询的时候  出现与关键字冲突
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private string CleanKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            { }
            else
            {
                bool isClean = false;
                while (!isClean)
                {
                    keyword = keyword.Trim();
                    if (keyword.EndsWith(" AND"))
                    {
                        keyword = string.Format("{0}and", keyword.Remove(keyword.Length - 3, 3));
                    }
                    else if (keyword.EndsWith(" OR"))
                    {
                        keyword = string.Format("{0}or", keyword.Remove(keyword.Length - 2, 2));
                    }
                    else if (keyword.StartsWith("AND "))
                    {
                        keyword = string.Format("and{0}", keyword.Substring(3));
                    }
                    else if (keyword.StartsWith("OR "))
                    {
                        keyword = string.Format("or{0}", keyword.Substring(2));
                    }
                    else if (keyword.Contains(" OR "))
                    {
                        keyword = keyword.Replace(" OR ", " or ");
                    }
                    else if (keyword.Contains(" AND "))
                    {
                        keyword = keyword.Replace(" AND ", " and ");
                    }
                    else
                        isClean = true;
                }

            }
            return QueryParser.Escape(keyword);
        }
        #endregion AnalyzerKey



    }
}