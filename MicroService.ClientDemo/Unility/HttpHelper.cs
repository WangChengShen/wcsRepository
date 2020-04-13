using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.ClientDem.Unility
{
    public class HttpHelper
    {
        public static string WebApiHttpGet(string url)
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
