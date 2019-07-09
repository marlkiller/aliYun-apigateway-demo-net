using aliyun_api_gateway_sdk.Constant;
using aliyun_api_gateway_sdk.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace myApp
{
    class Program
    {

        private const String appKey = "";
        private const String appSecret = "";
        private const String host = "";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            doGet();
            doPostString();
        }

        private static void doGet()
        {
            String path = "/sync_data/test";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_TEXT);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_TEXT);
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");


            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpGet(host, path, appKey, appSecret, 30000, headers, querys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPostString()
        {
            String bobyContent = "[{'time':'2019-05-22 21:45:00','unique_id':'ca2dc621789e4588a7e1d78eb5837537','floor_id':'636300233436576132','floor_name':'B1','vistor_num':5,'leaving_num':38,'position':'BeijingPuP_214_B1F_出口','mall_area':'21401'},{'time':'2019-05-22 22:00:00','unique_id':'0f898431fdc749e1b6ba91626614fd7f','floor_id':'636300233436576132','floor_name':'B1','vistor_num':3,'leaving_num':3,'position':'BeijingPuP_214_B1F_出口','mall_area':'21401'}]";

            String path = "/sync_data/sync_with_json";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            Dictionary<String, String> bodys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_JSON);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            //注意：如果有非Form形式数据(body中只有value，没有key)；如果body中是key/value形式数据，不要指定此行
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(Encoding.UTF8.GetBytes(bobyContent)));
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //注意：业务body部分
            bodys.Add("", bobyContent);

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpPost(host, path, appKey, appSecret, 30000, headers, querys, bodys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }
    }
}
