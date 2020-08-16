using System.Threading.Tasks;
using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Business.Core
{
    public interface IBalanceChecker
    {
        Task<bool> CanSaveBalance(int creationDay, decimal amount, AccountType accountType);
    }
}
