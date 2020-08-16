using CoreBanking.Database.Enum;
using System.Threading.Tasks;

namespace CoreBanking.Business.Core
{
    public interface IBalanceChecker
    {
        Task<bool> CanSaveBalance(decimal amount, AccountType accountType);
    }
}
