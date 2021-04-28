using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public partial class ApiService
    {
        public static readonly HttpClient Client = new HttpClient();

        public async Task<TResponse> Get<TResponse>(string url)
        {
            var req = new Request(url)
            {
                HttpMethod = HttpMethod.Get
            };
            return await ExtractRespnose<TResponse>(req);
        }

        public async Task<TResponse> Send<TResponse>(string url, HttpMethod type, object param = null)
        {
            var req = new Request(url)
            {
                HttpMethod = type,
                RequestContent = param
            };
            return await ExtractRespnose<TResponse>(req);
        }

        private async Task<TResponse> ExtractRespnose<TResponse>(Request request)
        {
            var res = await request.Send();

            if (typeof(TResponse).IsPrimitive || typeof(TResponse).Equals(typeof(string)))
                return (TResponse)Convert.ChangeType(res.ResponseText, typeof(TResponse));

            return res.GetObject<TResponse>();
        }
    }
}
