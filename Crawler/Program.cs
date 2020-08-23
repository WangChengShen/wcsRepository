using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;
namespace Crawler
{
    /// <summary>
    /// 网络爬虫
    /// 解析html的一个包HtmlAgilityPack，其原理是根据正则表达式找字符串
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://ke.qq.com/course/list/";
            string result = RequestUrl(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(result);
            string pagePath = "/html/body/section[1]/div/div[6]/a[@class='page-btn']";
            HtmlNodeCollection pageNodes = htmlDocument.DocumentNode.SelectNodes(pagePath);
            int maxPage = pageNodes.Select(p => Convert.ToInt32(p.InnerText)).Max();

            for (int page = 1; page <= maxPage; page++)
            {
                url = $"https://ke.qq.com/course/list?page={page}";
                getDataByUrl(url);
            }

            Console.WriteLine($"总共爬取数据{num}条");
        }

        static int num = 0;
        static void getDataByUrl(string url)
        {
            string result = RequestUrl(url);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(result);

            //xpath可以通过浏览器查看元素，右键选择copy xpath进行复制
            string xpath = "/html/body/section[1]/div/div[4]/ul/li";

            HtmlNodeCollection htmlNode = htmlDocument.DocumentNode.SelectNodes(xpath);
             
            foreach (var itemNodel in htmlNode)
            {
                num++;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(itemNodel.OuterHtml);

                //找当前li下面的a的xpath写法，也可以加一些属性条件，如：  "//*/a[@class='myA']", "//*/a[@name='myA']" 等

                HtmlNode aNote = document.DocumentNode.SelectSingleNode("//*/a");
                string id = aNote.Attributes["data-id"].Value;
                Console.WriteLine($"【{num}】课程id:{id}");
                HtmlNode imgNote = document.DocumentNode.SelectSingleNode("//*/a/img");
                string imgUrl = imgNote.Attributes["src"].Value;
                Console.WriteLine($"【{num}】课程图片:{imgUrl}");
                string title = imgNote.Attributes["title"].Value;
                Console.WriteLine($"【{num}】课程标题:{title}");
                Console.WriteLine("#############");
            }
        }
         
        static string RequestUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);

                var result = client.SendAsync(message);
                string content = result.Result.Content.ReadAsStringAsync().Result;

                return content;
            }
        }
    }
}
