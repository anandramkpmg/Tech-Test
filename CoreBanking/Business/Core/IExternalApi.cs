using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Business.Core
{
    public interface IExternalApi
    {
        bool CheckAccountBalance(decimal amount, AccountType accountType);
    }
}
