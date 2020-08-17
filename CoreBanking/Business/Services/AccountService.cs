using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreBanking.Services.Business.Core;
using CoreBanking.Services.Business.Interface;
using CoreBanking.Services.Database;
using CoreBanking.Services.Database.Models;
using CoreBanking.Services.Models;

namespace CoreBanking.Services.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDbContext _context;
        private readonly IBalanceChecker _balanceChecker;
        private readonly IMapper _mapper;
        public AccountService(IDbContext context, IBalanceChecker balanceChecker, IMapper mapper)
        {
            this._context = context;
            this._balanceChecker = balanceChecker;
            this._mapper = mapper;
        }

        public async Task<AccountModel> CreateAccount(AccountModel model)
        {
            if(IsUserExists(model)) { throw new Exception("User name already exist."); }

            if (!await _balanceChecker.CanSaveBalance(DateTime.Now.Day, model.Balance)) { throw new Exception("User cannot be saved with this balance."); }

            model.AccountType = _balanceChecker.GetAccountType(model.Balance);

            var addedCustomer = _context.Accounts.Add(_mapper.Map<Account>(model));
        
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountModel>(addedCustomer.Entity);
        }

        public async Task<AccountModel[]> GetAccounts()
        {
            var task = Task.Run(() => _context.Accounts.Select(customer => _mapper.Map<AccountModel>(customer)).ToArray());
            return await task;
        }

        public AccountModel[] GetAccounts(AccountModel[] accountModels, AddressModel address)
        {
            if (address == null || address.results.Length < 0) { return accountModels; }

            var fullAddress = address.results[0].location.city + " " + address.results[0].location.postcode;

            foreach (var account in accountModels)
            {
                account.Address = fullAddress;
            }

            return accountModels;
        }

        public async Task<AccountModel> UpdateAccount(AccountModel model)
        {
            if (IsUserExists(model)) { throw new Exception("User name already exist."); }

            var loadedAccount = _context.Accounts.FirstOrDefault(account => account.Id == model.Id);

            if (loadedAccount == null) { throw new Exception("User does not exist."); }

            var updatedAccount = _mapper.Map(model, loadedAccount);

            if (!await _balanceChecker.CanSaveBalance(DateTime.Now.Day, updatedAccount.Balance)) { throw new Exception("User cannot be saved with this balance."); }

            updatedAccount.AccountType = _balanceChecker.GetAccountType(updatedAccount.Balance);

            await _context.SaveChangesAsync();

            return _mapper.Map<AccountModel>(updatedAccount);
        }

        private bool IsUserExists(AccountModel model)
        {
            return _context.Accounts.FirstOrDefault(acc =>
                       acc.FirstName == model.FirstName && acc.LastName == model.LastName) != null;
        }
    }
}
