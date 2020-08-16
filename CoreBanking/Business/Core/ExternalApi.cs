using CoreBanking.Database.Enum;

namespace CoreBanking.Business.Core
{
    public class ExternalApi : IExternalApi
    {
        public bool CheckAccountBalance(decimal amount, AccountType accountType)
        {
            return (amount > 1000000 && accountType == AccountType.Gold);
        }
    }
}
