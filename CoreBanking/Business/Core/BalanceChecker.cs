using System;
using System.Threading.Tasks;
using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Business.Core
{
    public class BalanceChecker : IBalanceChecker
    {
        private readonly IProcess _process;
        private readonly IPersistence _persistence;
        private readonly IExternalApi _externalApi;

        public BalanceChecker(IProcess process, IPersistence persistence, IExternalApi externalApi)
        {
            _process = process;
            _persistence = persistence;
            _externalApi = externalApi;
        }

        public async Task<bool> CanSaveBalance(decimal amount, AccountType accountType)
        {
            switch (amount)
            {
                case { } amt when amount < 10:
                    await Task.Run(() => { _process.Process10(); });
                    return true;

                case { } amt when (amount > 50 && amount <= 100000) && DateTime.Now.Day > 15:
                    return await Task.Run(_persistence.GetInfo);

                case { } amt when amount > 100000:
                    return await Task.Run(() => _externalApi.CheckAccountBalance(amount, accountType));

                default:
                    return false;
            }
        }
    }

}