using System.Threading.Tasks;
using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Business.Core
{
    public interface IBalanceChecker
    {
        Task<bool> CanSaveBalance(decimal amount, AccountType accountType);
    }
}
