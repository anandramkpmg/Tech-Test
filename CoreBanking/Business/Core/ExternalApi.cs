using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Business.Core
{
    public class ExternalApi : IExternalApi
    {
        public bool CheckAccountBalance(decimal amount, AccountType accountType)
        {
            return (amount > 100000 && accountType == AccountType.Gold);
        }
    }
}
