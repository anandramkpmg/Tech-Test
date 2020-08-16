using CoreBanking.Business.Interface;
using CoreBanking.Database;
using CoreBanking.Models;
using AutoMapper;
using System.Linq;
using CoreBanking.Database.Models;
using System.Threading.Tasks;
using System;
using CoreBanking.Business.Core;

namespace CoreBanking.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBalanceChecker _balanceChecker;
        private readonly IMapper _mapper;
        public AccountService(ApplicationDbContext context, IBalanceChecker balanceChecker, IMapper mapper)
        {
            this._context = context;
            this._balanceChecker = balanceChecker;
            this._mapper = mapper;
        }

        public async Task<AccountModel> CreateAccount(AccountModel model)
        {
            var addedCustomer = _context.Accounts.Add(_mapper.Map<Account>(model));

            if (!await _balanceChecker.CanSaveBalance(model.Balance, model.AccountType)) { throw new Exception("User cannot be saved with this balance."); }

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
            var loadedAccount = _context.Accounts.FirstOrDefault(account => account.Id == model.Id);

            if (loadedAccount == null) { throw new Exception("User does not exist."); }

            var updatedAccount = _mapper.Map(model, loadedAccount);

            if (!await _balanceChecker.CanSaveBalance(updatedAccount.Balance, updatedAccount.AccountType)) { throw new Exception("User cannot be saved with this balance."); }

            await _context.SaveChangesAsync();

            return _mapper.Map<AccountModel>(updatedAccount);
        }
    }
}
