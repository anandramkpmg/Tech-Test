using AutoMapper;
using CoreBanking.Services.Business.Core;
using CoreBanking.Services.Database;
using Moq;
using NUnit.Framework;

namespace CoreBankingTests.ServiceTests
{
    public class AccountServiceTests
    {
        private Mock<IDbContext> _mockDbContext;
        private Mock<IBalanceChecker> _mockBalanceChecker;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<IDbContext>();
            _mockBalanceChecker = new Mock<IBalanceChecker>();
            _mockMapper = new Mock<IMapper>();
        }
       
        [Test]
        public void CanSaveBalance_BalanceLessThan10_ReturnsTrue()
        {
            //var accountService = new AccountService(_mockDbContext.Object,  _mockBalanceChecker.Object, _mockMapper.Object);
        }
    }
}
