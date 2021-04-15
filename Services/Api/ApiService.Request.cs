using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public partial class ApiService
    {
        public partial class Request
        {
            public Request(string relativeUrl) => RelativeUrl = relativeUrl;
            public string HttpMethod { get; set; } = RequestMethods.GET;
            public string RelativeUrl { get; set; }
            public async Task<Response> Send()
            {
                try
                {
                    var req = new HttpRequestMessage(new HttpMethod(HttpMethod), RelativeUrl);

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
        }

        public class RequestMethods
        {
            public const string GET = "GET";

            public const string POST = "POST";

            public const string PUT = "PUT";

            public const string DELETE = "DELETE";
        }
    }
}
