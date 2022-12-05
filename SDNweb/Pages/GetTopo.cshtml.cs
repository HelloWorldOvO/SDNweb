using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SDNweb.Pages
{
    public class GetTopoModel : PageModel
    {
        private readonly ILogger<GetTopoModel> _logger;

        public GetTopoModel(ILogger<GetTopoModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string dateTime = DateTime.Now.ToString("d", new CultureInfo("en-US"));
            ViewData["TimeStamp"] = dateTime;
        }

        public void OnPostGetData()
        {
            String hosts = GetONOSInformation("hosts");
            String Devices = GetONOSInformation("devices");
            String Links = GetONOSInformation("links");
            String Deviceids = DevicestoDevicesid(Devices);

            ViewData["Devices"] = Devices;
            ViewData["Hosts"] = hosts;
            ViewData["Links"] = Links;
            ViewData["Deviceids"] = Deviceids;


            string dateTime = DateTime.Now.ToString("d", new CultureInfo("en-US"));
            ViewData["TimeStamp"] = dateTime;

            ViewData["Test"] = "OnPostGetTopo execute complete";
        }

        public String GetONOSInformation(String getItem)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };

            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://192.168.98.142:8181/onos/v1/" + getItem);
                var result = client.GetAsync(endpoint).Result;
                var jsonstring = result.Content.ReadAsStringAsync().Result;
                return jsonstring;
            }
        }

        public String DevicestoDevicesid(String devices)
        {
            List<string> deviceids = new List<string>();

            foreach (var item in devices.Replace("}{", "}|{").Split('|'))
            {
                String line = item;
                //Console.WriteLine(line);

                dynamic dline = JObject.Parse(line);
                String device = Convert.ToString(dline.devices[0]);
                dynamic ddevice = JObject.Parse(device);
                //Console.WriteLine("test:" + ddevice.id);
                String deviceid = Convert.ToString(ddevice.id);
                deviceids.Add(deviceid);
            }


            var result = String.Join(", ", deviceids.ToArray());
            return result;
        }


    }
}