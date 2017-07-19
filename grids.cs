using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;

namespace Теги
{
    class Program
    {
        static void Main(string[] args)
        {
            const string VERSION = "v1";
            WebClient wc = new WebClient();
            {
                // Авторизация
                wc.Encoding = Encoding.UTF8;
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";
                var data = wc.UploadString($"https://api.eldis24.ru/api/{VERSION}/users/login", "login=uLoginHerepass=uPasswordHere");
                var cookies = wc.ResponseHeaders[HttpResponseHeader.SetCookie];

                Regex reg = new Regex("access_token=(?<Token>[^;]+)");
                var match = reg.Match(cookies);
                var token = match.Groups["Token"];

                wc.Headers.Add(HttpRequestHeader.Cookie, $"access_token={token}");
              



                var json = JObject.FromObject(new
                {
                    request = new
                    {
                        tags = new
                        {
                            change = new
                            {
                                records = new[]
                                {
                                    //хлебопекарня                              
                                    new {id="ca2a360c-56fa-45d2-b02d-4201651cb394"}//тут должен быть Guid ?
                                },
                                add = new[]
                                {
                                    
                                    new {id=(Guid?)null, name="exampleTag"}
                                },
                                delete = new[]
                                {
                                    new {id="0d86b742-071d-4ec9-9599-be30b5ea847a",name="test"}
                                }
                                //сообщения???
                            }
                        }
                    },
                });
                wc.Headers["Content-Type"] = "text/plain";
                wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";
                var str = json.ToString();
                var response = wc.UploadString($"https://api.eldis24.ru/api/v1/tags/change?entity=objects/list", str);

            }
        }
    }
}
