using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public partial class ApiService
    {
        public static readonly HttpClient Client = new HttpClient();
    }
}
