using System;
using System.Linq;
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
        public void GetAccounts_AccountsFound_ReturnsAllAccounts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Accounts.Add(new Account()
                { Id = 1, AccountType = AccountType.Bronze, Balance = 2, FirstName = "James", LastName = "Cook" });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var accountService = new AccountService(context, _mockBalanceChecker.Object, _mockMapper.Object);
                var accounts = accountService.GetAccounts().Result;
                Assert.AreEqual(context.Accounts.Count(), accounts.Length);
            }
        }

        [Test]
        public void CreateAccount_ValidAccount_NewAccountAdded()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Accounts.Add(new Account()
                { Id = 2, AccountType = AccountType.Bronze, Balance = 2, FirstName = "James", LastName = "Cook" });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var accountModel = new AccountModel() { AccountType = AccountType.Bronze, Balance = 3, FirstName = "David", LastName = "Cook" };

                _mockBalanceChecker.Setup(x => x.CanSaveBalance(DateTime.Now.Day, accountModel.Balance))
                    .Returns(Task.FromResult(true));

                _mockBalanceChecker.Setup(x => x.GetAccountType(accountModel.Balance)).Returns(AccountType.Silver);

                _mockMapper.Setup(x => x.Map<Account>(It.IsAny<AccountModel>()))
                    .Returns((AccountModel source) => new Account()
                    {
                        AccountType = source.AccountType,
                        Balance = source.Balance,
                        FirstName = source.FirstName,
                        LastName = source.LastName
                    });

                var accountService = new AccountService(context, _mockBalanceChecker.Object, _mockMapper.Object);

                var numberOfAccounts = context.Accounts.Count();

                var account = accountService.CreateAccount(accountModel).Result;
                var accounts = accountService.GetAccounts().Result;
                Assert.AreEqual(numberOfAccounts + 1, accounts.Length);
            }
        }

        [Test]
        public void CreateAccount_UserNameExists_ThrowsException()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Accounts.Add(new Account()
                { Id = 3, AccountType = AccountType.Bronze, Balance = 2, FirstName = "James", LastName = "Cook" });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var accountModel = new AccountModel() {Id =3, AccountType = AccountType.Bronze, Balance = 3, FirstName = "James", LastName = "Cook" };

                var accountService = new AccountService(context, _mockBalanceChecker.Object, _mockMapper.Object);

                Assert.That(async () => await accountService.CreateAccount(accountModel), Throws.Exception);
            }
        }
    }
}
