using CoreBanking.Business.Interface;
using CoreBanking.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreBanking.Business.Services
{
    public class PostcodeService : IPostcodeService
    {
        private static string url = "https://randomuser.me/api/?nat=gb";
        public async Task<AddressModel> GetAddress1()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AddressModel>(apiResponse);
                }
            }
        }

        public async Task<string> GetAddress()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                   return await response.Content.ReadAsStringAsync();
                    //return JsonConvert.DeserializeObject<AddressModel>(apiResponse);
                }
            }
        }
    }
}
