using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBanking.Services.Business.Accounts;
using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Business.Core
{
    public class BalanceChecker : IBalanceChecker
    {
        private readonly IProcess _process;
        private readonly IPersistence _persistence;
        private readonly IExternalApi _externalApi;
        private readonly List<Account> _accounts = new List<Account>();

        public BalanceChecker(IProcess process, IPersistence persistence, IExternalApi externalApi)
        {
            _process = process;
            _persistence = persistence;
            _externalApi = externalApi;
            BuildAccountTypes();
        }

        private void BuildAccountTypes()
        {
            _accounts.Add(new Silver(0, 50000));
            _accounts.Add(new Bronze(50001, 100000));
            _accounts.Add(new Gold(100000, int.MaxValue));
        }

        public AccountType GetAccountType(decimal balance)
        {
            foreach (var account in _accounts)
            {
                if (balance >= account.MinBalance && balance <= account.MaxBalance)
                {
                    return account.Type;
                }
            }

            return AccountType.NotSupported;
        }

        public async Task<bool> CanSaveBalance(int creationDay, decimal balance)
        {
            switch (balance)
            {
                case { } when balance < 10:
                    await Task.Run(() => { _process.Process10(); });
                    return true;

                case { } when (balance >= 10 && balance <= 50):
                    return true;

                case { } when (balance > 50 && balance <= 100000) && creationDay > 15:
                    return await Task.Run(_persistence.GetInfo);

                case { } when balance > 100000:
                    return await Task.Run(() => _externalApi.CheckAccountBalance(balance, GetAccountType(balance)));

                default:
                    return false;
            }
        }
    }
}