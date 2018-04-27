using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.Web.Http;

namespace LightWeather.Services {
    public static class WeatherService {
        private const string apiUrlJson = "https://www.sojson.com/open/api/weather/json.shtml?city=";
        private const string apiUrlXml = "https://www.sojson.com/open/api/weather/xml.shtml?city=";

        private static async Task<string> Get(string url) {
            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header)) {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header)) {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri(url);

            //Send the GET request asynchronously and retrieve the response as a string.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                // httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                httpResponseBody = null;
            }
            return httpResponseBody;
        }

        public static async Task<JsonObject> GetJson(string city) {
            string res = await Get(apiUrlJson + city);
            if (res == null) {
                return null;
            }
            return JsonObject.Parse(res);
        }

        public static async Task<XmlDocument> GetXml(string city) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(await Get(apiUrlXml + city));
            return doc;
        }
    }
}
