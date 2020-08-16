using CoreBanking.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreBanking.Business.Interface
{
    public interface IPostcodeService
    {
        public Task<string> GetAddress();

        public Task<AddressModel> GetAddress1();
    }
}
