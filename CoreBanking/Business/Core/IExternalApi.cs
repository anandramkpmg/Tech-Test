using CoreBanking.Database.Enum;

namespace CoreBanking.Business.Core
{
    public interface IExternalApi
    {
        bool CheckAccountBalance(decimal amount, AccountType accountType);
    }
}
