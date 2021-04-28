using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public partial class ApiService
    {
        public partial class Request
        {
            object jsonData;
            public Request(string relativeUrl) => RelativeUrl = relativeUrl;
            public HttpMethod HttpMethod { get; set; }
            public string RelativeUrl { get; set; }
            public string ContentType { get; set; }
            public string RequestData { get; set; }
            public object RequestContent
            {
                get => jsonData;
                set
                {
                    jsonData = value;
                    if (value != null)
                    {
                        ContentType = "application/json";
                        RequestData = JsonConvert.SerializeObject(value);
                    }
                }
            }

            public async Task<Response> Send()
            {
                try
                {
                    var req = new HttpRequestMessage(HttpMethod, RelativeUrl);

                    req.Content = new StringContent(RequestData.OrEmpty(), Encoding.UTF8, GetContentType());

                    var response = await Client.SendAsync(req);

                    return new Response
                    {
                        ResponseCode = response.StatusCode,
                        ResponseText = await response.Content.ReadAsStringAsync()
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http {HttpMethod} failed -> {RelativeUrl}", ex);
                }
            }

            public string GetContentType()
            {
                return ContentType.Or("application/x-www-form-urlencoded".Unless(HttpMethod == HttpMethod.Get));
            }
        }
    }
}
