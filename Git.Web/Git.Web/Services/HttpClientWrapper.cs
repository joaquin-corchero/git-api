using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Git.Web.Services
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> Get(string address, object queryObject);
    }

    public class HttpClientWrapper : IHttpClient
    {
        public async Task<HttpResponseMessage> Get(string address, object queryObject)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = queryObject == null ? address : $"{address}/{JsonConvert.SerializeObject(queryObject)}";
                
                client.BaseAddress = new Uri(address);

                return await client.GetAsync(address);
            }
        }

    }

}
