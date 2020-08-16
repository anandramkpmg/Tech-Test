using CoreBanking.Services.Business.Core;
using CoreBanking.Services.Database.Enum;
using Moq;
using NUnit.Framework;

namespace CoreBankingTests.BusinessTests
{
    public class BalanceCheckerTests
    {
        private Mock<IProcess> _processMock;

        private Mock<IPersistence> _persistenceMock;

        private Mock<IExternalApi> _externalApiMock;

        [SetUp]
        public void Setup()
        {
            _processMock = new Mock<IProcess>();
            _persistenceMock = new Mock<IPersistence>();
            _externalApiMock = new Mock<IExternalApi>();
        }

        [Test]
        public void CanSaveBalance_BalanceLessThan10_ReturnsTrue()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);

            Assert.IsTrue(balanceChecker.CanSaveBalance(16, 1, AccountType.Bronze).Result);
            _processMock.Verify(expression: x=> x.Process10(), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceBetween50And100000_ReturnsTrue()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _persistenceMock.Setup(x => x.GetInfo()).Returns(true);
          
            Assert.IsTrue(balanceChecker.CanSaveBalance(16,51, AccountType.Bronze).Result);
            _persistenceMock.Verify(expression: x => x.GetInfo(), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceBetween50And100000AndDayFallsBefore15_ReturnsFalse()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _persistenceMock.Setup(x => x.GetInfo()).Returns(true);
            
            Assert.IsFalse(balanceChecker.CanSaveBalance(14, 51, AccountType.Bronze).Result);
            _persistenceMock.Verify(expression: x => x.GetInfo(), Times.Never);
        }

        [Test]
        public void CanSaveBalance_BalanceBetween50And100000PersistReturnsFalse_PersistanceCalledOnce()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _persistenceMock.Setup(x => x.GetInfo()).Returns(false);
            
            Assert.IsFalse(balanceChecker.CanSaveBalance(16, 51, AccountType.Bronze).Result);
            _persistenceMock.Verify(expression: x => x.GetInfo(), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceGreater100000PersistReturnsFalse_ExternalApiReturnsFalse()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _externalApiMock.Setup(x => x.CheckAccountBalance(100001, AccountType.Bronze)).Returns(false);

            Assert.IsFalse(balanceChecker.CanSaveBalance(16, 100001, AccountType.Bronze).Result);
            _externalApiMock.Verify(expression: x => x.CheckAccountBalance(100001, AccountType.Bronze), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceGreater100000PersistReturnsFalse_ExternalApiReturnsTrue()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _externalApiMock.Setup(x => x.CheckAccountBalance(100001, AccountType.Gold)).Returns(true);

            Assert.IsTrue(balanceChecker.CanSaveBalance(16, 100001, AccountType.Gold).Result);
            _externalApiMock.Verify(expression: x => x.CheckAccountBalance(100001, AccountType.Gold), Times.Once);
        }
    }
}