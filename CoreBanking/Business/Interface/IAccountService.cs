using System.Threading.Tasks;
using CoreBanking.Services.Models;

namespace CoreBanking.Services.Business.Interface
{
    public interface IAccountService
    {
        Task<AccountModel[]> GetAccounts();
        Task<AccountModel> CreateAccount(AccountModel model);
        AccountModel[] GetAccounts(AccountModel[] accountModels, AddressModel address);
        Task<AccountModel> UpdateAccount(AccountModel model);
    }
}
