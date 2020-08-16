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

        public async Task<bool> CanSaveBalance(int creationDay, decimal amount, AccountType accountType)
        {
            switch (amount)
            {
                case { } when amount < 10:
                    await Task.Run(() => { _process.Process10(); });
                    return true;

                case { } when (amount >= 10 && amount <= 50):
                    return true;

                case { } when (amount > 50 && amount <= 100000) && creationDay > 15:
                    return await Task.Run(_persistence.GetInfo);

                case { } when amount > 100000:
                    return await Task.Run(() => _externalApi.CheckAccountBalance(amount, accountType));

                default:
                    return false;
            }
        }
    }
}