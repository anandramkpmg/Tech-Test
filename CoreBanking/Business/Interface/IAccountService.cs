using CoreBanking.Models;
using System.Threading.Tasks;

namespace CoreBanking.Business.Interface
{
    public interface IAccountService
    {
        Task<AccountModel[]> GetAccounts();
        Task<AccountModel> CreateAccount(AccountModel model);
        AccountModel[] GetAccounts(AccountModel[] accountModels, AddressModel address);
    }
}
