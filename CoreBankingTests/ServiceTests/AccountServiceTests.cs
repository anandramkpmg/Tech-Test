using System.Threading.Tasks;
using AutoMapper;
using CoreBanking.Services.Business.Core;
using CoreBanking.Services.Business.Services;
using CoreBanking.Services.Database.Enum;
using CoreBanking.Services.Database.Models;
using CoreBanking.Services.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ApplicationDbContext = CoreBanking.Services.Database.ApplicationDbContext;

namespace CoreBankingTests.ServiceTests
{
    public class AccountServiceTests
    {
        private Mock<IBalanceChecker> _mockBalanceChecker;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockBalanceChecker = new Mock<IBalanceChecker>();
            _mockMapper = new Mock<IMapper>();
        }

        [Test]
        public void GetAccounts_AccountsFound_Success()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Accounts.Add(new Account() { Id = 1, AccountType = AccountType.Bronze, Balance = 2, FirstName = "James", LastName = "Cook" });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var accountService = new AccountService(context, _mockBalanceChecker.Object, _mockMapper.Object);
                var accounts = accountService.GetAccounts().Result;
                Assert.AreEqual(1, accounts.Length);
            }
        }

        [Test]
        [Ignore("wip")]
        public void Add()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Accounts.Add(new Account() { Id = 1, AccountType = AccountType.Bronze, Balance = 2, FirstName = "James", LastName = "Cook" });
                context.SaveChanges();
            }

            _mockBalanceChecker.Setup(x => x.CanSaveBalance(16, 12, AccountType.Silver)).Returns(new Task<bool>(() => true));

            using (var context = new ApplicationDbContext(options))
            {
                var accountService = new AccountService(context, _mockBalanceChecker.Object, _mockMapper.Object);
                var accountModel = new AccountModel() { AccountType = AccountType.Bronze, Balance = 3, FirstName = "David", LastName = "Cook" };
                var account = accountService.CreateAccount(accountModel).Result;
                var accounts = accountService.GetAccounts().Result;
                Assert.IsFalse(account.FirstName == "David");
                Assert.AreEqual(2, accounts.Length);
            }
        }


        [Test]
        [Ignore("wip")]
        public void Update()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Accounts.Add(new Account() { Id = 1, AccountType = AccountType.Bronze, Balance = 2, FirstName = "James", LastName = "Cook" });
                context.SaveChanges();
            }

            _mockBalanceChecker.Setup(x => x.CanSaveBalance(16, 12, AccountType.Silver)).Returns(new Task<bool>(() => true));

            using (var context = new ApplicationDbContext(options))
            {

                var accountModel = new AccountModel() { AccountType = AccountType.Bronze, Id = 1, Balance = 3, FirstName = "Jimmy", LastName = "Cook" };
                var updatedAccount = new Account() { Id = 1, AccountType = AccountType.Bronze, Balance = 3, FirstName = "Jimmy", LastName = "Cook" };

                _mockMapper.Setup((x => x.Map(accountModel, updatedAccount))).Returns(updatedAccount);

                var accountService = new AccountService(context, _mockBalanceChecker.Object, _mockMapper.Object);

                var account = accountService.UpdateAccount(accountModel).Result;


                var accounts = accountService.GetAccounts().Result;
                Assert.IsTrue(account.FirstName == "Jimmy" && account.Balance == 3);
                Assert.AreEqual(1, accounts.Length);
            }
        }
    }
}
