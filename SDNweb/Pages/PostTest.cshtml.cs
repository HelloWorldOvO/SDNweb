using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using static System.Net.WebRequestMethods;
using System.Text.Json.Serialization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Nodes;


namespace SDNweb.Pages
{
    public class PostTestModel : PageModel
    {
       
        private readonly ILogger<PostTestModel> _logger;

        public PostTestModel(ILogger<PostTestModel> logger)
        {
            _logger = logger;
        }

        public class Criterion
        {
            public string type { get; set; }
            public string ethType { get; set; }
            public string ip { get; set; }
        }
        public class Instructions
        {
            public string type { get; set; }            
            public int port { get; set; }
        }

        public void OnGet()
        {           
           string dateTime = DateTime.Now.ToString("d", new CultureInfo("en-US"));
           ViewData["TimeStamp"] = dateTime;
        }

        public void OnPostPostTest(string WebIP,string deviceID, string appID, string stream)
        {            
            string dateTime = DateTime.Now.ToString("d", new CultureInfo("en-US"));            
            ViewData["TimeStamp"] = dateTime;
            POSTONOSInformation("d");
        }

        public async void POSTONOSInformation(String getItem)
        {
            List<string> deferred_ID = new List<string>() { };
            List<Instructions> instructionsID = new List<Instructions>()
            {
                new Instructions(){type = "OUTPUT",port=1}                
            };            

            List<Criterion> criteria_ID = new List<Criterion>() 
            {
                new Criterion(){type = "ETH_TYPE",ethType = "0x0800"},
                new Criterion(){type = "IPV4_DST",ip = "10.0.0.1/32"}
            };
            var senddata = new
            {
                priority = 4001,
                timeout = 0,
                isPermanent = true,
                deviceId = "of:0000000000000001",
                treatment = new
                {
                    deferred = deferred_ID,
                    instructions = instructionsID
                },
                selector = new
                {
                    criteria = criteria_ID

                }
            };

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };

            var json = JsonConvert.SerializeObject(senddata);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "http://192.168.98.142:8181/onos/v1/flows/of%3A0000000000000001?appId=hello";
            using var client = new HttpClient(httpClientHandler);
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }
    }
}
