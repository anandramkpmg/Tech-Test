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

            Assert.IsTrue(balanceChecker.CanSaveBalance(16, 1).Result);
            _processMock.Verify(expression: x=> x.Process10(), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceBetween50And100000_ReturnsTrue()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _persistenceMock.Setup(x => x.GetInfo()).Returns(true);
          
            Assert.IsTrue(balanceChecker.CanSaveBalance(16,51).Result);
            _persistenceMock.Verify(expression: x => x.GetInfo(), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceBetween50And100000AndDayFallsBefore15_ReturnsFalse()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _persistenceMock.Setup(x => x.GetInfo()).Returns(true);
            
            Assert.IsFalse(balanceChecker.CanSaveBalance(14, 51).Result);
            _persistenceMock.Verify(expression: x => x.GetInfo(), Times.Never);
        }

        [Test]
        public void CanSaveBalance_BalanceBetween50And100000PersistReturnsFalse_PersistanceCalledOnce()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _persistenceMock.Setup(x => x.GetInfo()).Returns(false);
            
            Assert.IsFalse(balanceChecker.CanSaveBalance(16, 51).Result);
            _persistenceMock.Verify(expression: x => x.GetInfo(), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceGreater100000ExternalApiReturnsFalse_ExternalApiReturnsFalse()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _externalApiMock.Setup(x => x.CheckAccountBalance(100001, AccountType.Gold)).Returns(false);

            Assert.IsFalse(balanceChecker.CanSaveBalance(16, 100001).Result);
            _externalApiMock.Verify(expression: x => x.CheckAccountBalance(100001, AccountType.Gold), Times.Once);
        }

        [Test]
        public void CanSaveBalance_BalanceGreater100000ExternalApiReturnsFalse_ExternalApiReturnsTrue()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            _externalApiMock.Setup(x => x.CheckAccountBalance(100001, AccountType.Gold)).Returns(true);

            Assert.IsTrue(balanceChecker.CanSaveBalance(16, 100001).Result);
            _externalApiMock.Verify(expression: x => x.CheckAccountBalance(100001, AccountType.Gold), Times.Once);
        }

        [Test]
        public void GetAccountType_1_Silver()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            Assert.AreEqual(balanceChecker.GetAccountType(1), AccountType.Silver);
        }

        [Test]
        public void GetAccountType_50000_Bronze()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            Assert.AreEqual(balanceChecker.GetAccountType(50000), AccountType.Silver);
        }

        [Test]
        public void GetAccountType_50001_Bronze()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            Assert.AreEqual(balanceChecker.GetAccountType(50001), AccountType.Bronze);
        }

        [Test]
        public void GetAccountType_100001_Bronze()
        {
            var balanceChecker = new BalanceChecker(_processMock.Object, _persistenceMock.Object, _externalApiMock.Object);
            Assert.AreEqual(balanceChecker.GetAccountType(100001), AccountType.Gold);
        }
    }
}