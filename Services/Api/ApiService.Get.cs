using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public partial class ApiService
    {
        public async Task<TResponse> Get<TResponse>(string url)
        {
            var req = new Request(url) { HttpMethod = RequestMethods.GET };
            var res = await req.Send();
            return res.GetObject<TResponse>();
        }
    }
}
