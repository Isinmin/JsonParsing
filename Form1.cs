using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();


        }

        public void LoadObjects()
        {
            var search = string.IsNullOrEmpty(textBox1.Text) ? "" : $"?search={textBox1.Text}";
            dataGridView1.Rows.Clear();

             const string version = "v1";

            WebClient wc = new WebClient();
            {
                wc.Encoding = Encoding.UTF8;
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";
                var data = wc.UploadString($"https://api.eldis24.ru/api/{version}/users/login", "login=UloginHere.com&password=UpasswordHere");

                var cookies = wc.ResponseHeaders[HttpResponseHeader.SetCookie];

                Regex reg = new Regex("access_token=(?<Token>[^;]+)");
                var match = reg.Match(cookies);
                var token = match.Groups["Token"];

                wc.Headers.Add(HttpRequestHeader.Cookie, $"access_token={token}");
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";

                var response = wc.UploadString($"https://api.eldis24.ru/api/{version}/objects/list{search}", "");
                var jObject = JObject.Parse(response)["response"]["objects"]["list"];



                if (jObject != null)
                {
                    var objects = jObject.Select(a => new
                    {
                        Id = (Guid)a["id"],
                        IsWinterMode = (bool)a["isWinterMode"],
                        Address = (string)a["address"],
                        Name = (string)a["name"],
                        Consumer = (string)a["consumer"],
                        CreatedOn = ToDateTime((long)a["createdOn"])
                    }).ToList();

                    foreach (var item in objects)
                    {
                        dataGridView1.Rows.Add(item.Id, item.IsWinterMode, item.Address, item.Name, item.Consumer, item.CreatedOn);
                    }
                }

                data = wc.DownloadString($"https://api.eldis24.ru/api/{version}/users/logout");

            }
        }

        public DateTime ToDateTime(long date)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(date);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadObjects();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var row = dataGridView1.SelectedRows[0];
            dataGridView2.Rows.Clear();

            string objID=$"?id={ dataGridView1[0,row.Index ].Value.ToString()}";
            //MessageBox.Show(objID);
            WebClient wc = new WebClient();
            {
                var version = "v1"; 
                wc.Encoding = Encoding.UTF8;
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";
                var data = wc.UploadString($"https://api.eldis24.ru/api/{version}/users/login", "login=UloginHere.com&password=UpasswordHere");

                var cookies = wc.ResponseHeaders[HttpResponseHeader.SetCookie];

                Regex reg = new Regex("access_token=(?<Token>[^;]+)");
                var match = reg.Match(cookies);
                var token = match.Groups["Token"];
                wc.Headers.Add(HttpRequestHeader.Cookie, $"access_token={token}");

                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                wc.Headers["key"] = "eVCX2lzO7prHREze1TY2wglCAaHg7eHUNqJYUMi5Ps8zbzpLNLKqvKyenU5v6pPfMj35PG==";
     
                var response = wc.DownloadString($"https://api.eldis24.ru/api/{version}/objects/get{objID}");

                var jObject = JObject.Parse(response)["response"]["objects"]["get"];



                if (jObject != null)
                {
                    var typeObject = (string)jObject["typeObject"]["name"];

                    dataGridView2.Rows.Add((string)jObject["title"], (string)jObject["address"], (string)jObject["name"]);

                }

                data = wc.DownloadString($"https://api.eldis24.ru/api/{version}/users/logout");

            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
