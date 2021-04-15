using Newtonsoft.Json;
using System;
using System.Net;

namespace Olive.ApiClient.Services
{
    public partial class ApiService
    {
        public class Response
        {
            public string ResponseText { get; set; }
            public HttpStatusCode ResponseCode { get; set; }
            public TResponse GetObject<TResponse>()
            {
                try
                {
                    if (ResponseText.HasValue())
                        return JsonConvert.DeserializeObject<TResponse>(ResponseText);

                    return default;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to convert API response to " + typeof(TResponse).GetCSharpName(), ex);
                }
            }
        }
    }
}
