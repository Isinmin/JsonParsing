using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace grids
{
    class Program
    {
        static void Main(string[] args)
        {
            const string VERSION = "v1";
            WebClient wc = new WebClient();
            
                // Авторизация
                wc.Encoding = Encoding.UTF8;
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";
                var data = wc.UploadString($"https://api.eldis24.ru/api/{VERSION}/users/login", "login=UloginHere&password=UpasswordHere");
                var cookies = wc.ResponseHeaders[HttpResponseHeader.SetCookie];

                Regex reg = new Regex("access_token=(?<Token>[^;]+)");
                var match = reg.Match(cookies);
                var token = match.Groups["Token"];

                wc.Headers.Add(HttpRequestHeader.Cookie, $"access_token={token}");
            

            //json
            var json = JObject.FromObject(new
            {
                request = new
                {
                    columnMappings = new
                    {
                        edit = new[]
                        {
                            new {name="address",ordinalPosition=1,width="500",sorted="ACD"},
                            new {name="name",ordinalPosition=2,width="300",sorted="ACD"}

                        }
                    }
                }
            });

            wc.Headers["Content-Type"] = "text/plain";
            wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";
            var str = json.ToString();
            var response = wc.UploadString($"https://api.eldis24.ru/api/v1/columnMappings/edit?entity=objects/list", str);



            //выход
            data = wc.DownloadString($"https://api.eldis24.ru/api/{VERSION}/users/logout");
        }
    }
}
