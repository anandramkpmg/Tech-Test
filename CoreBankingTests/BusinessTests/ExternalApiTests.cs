using CoreBanking.Services.Business.Core;
using CoreBanking.Services.Database.Enum;
using NUnit.Framework;

namespace CoreBankingTests.BusinessTests
{
    public class ExternalApiTests
    {
        [Test]
        public void CanSaveBalance_BalanceLessThan100000_ReturnsFalse()
        {
            var externalApi = new ExternalApi();
            Assert.IsFalse(externalApi.CheckAccountBalance(100, AccountType.Bronze));
        }

        [Test]
        public void CanSaveBalance_GoldAccountType_ReturnsFalse()
        {
            var externalApi = new ExternalApi();
            Assert.IsFalse(externalApi.CheckAccountBalance(100000, AccountType.Gold));
        }

        [Test]
        public void CanSaveBalance_GoldAccountTypeBalanceGreaterThan100000_ReturnsTrue()
        {
            var externalApi = new ExternalApi();
            Assert.IsTrue(externalApi.CheckAccountBalance(100001, AccountType.Gold));
        }
    }
}
