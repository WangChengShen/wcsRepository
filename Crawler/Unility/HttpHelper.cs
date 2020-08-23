using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Crawler
{
    public class HttpHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="param"></param>
        /// <param name="serviceCode"></param>
        /// <returns></returns>
        public static string PostWebApi<P>(string apiUrl, P param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl);
            //request.KeepAlive = false;
            request.Timeout = 300000;
            request.Method = "post";
            byte[] postdatabyte = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            request.ContentLength = postdatabyte.Length;
            request.ContentType = "application/json;charset=UTF-8";
            //request.Headers.Add("appId", AppId);

            Stream stream = request.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
            {
                Stream myResponseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(myResponseStream);
                string jsonStr = streamReader.ReadToEnd();

                streamReader.Close();
                myResponseStream.Close();
                return jsonStr;
            }
        }

        public static string PostWebApi2<P>(string apiUrl, P param)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                //request.Content = new FormUrlEncodedContent(dic.Select(s => s));

                request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(param));
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //request.Headers.Add("appId", AppId);
                //request.Headers.Add("serviceCode", serviceCode);
                //request.Content.Headers.Add("Content-Signature", $"HMAC-SHA1 {HMACSHA1Text(Newtonsoft.Json.JsonConvert.SerializeObject(param), $"{AppSecret}{serviceCode}")}");

                var result = httpClient.SendAsync(request).Result;
                var resultStr = result.Content.ReadAsStringAsync().Result;

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception(resultStr);
                }
                return resultStr;
            }
        }
    }
}
