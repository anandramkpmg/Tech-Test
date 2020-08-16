using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Models
{
    public class AccountModel
    {
        private AccountType _accountType;
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public AccountType AccountType { get => GetAccountType(); set => _accountType = value; }
        public string Address { get; set; }
        private AccountType GetAccountType()
        {
            return (Balance <= 50000) ? AccountType.Silver :
                   (Balance > 50000 && Balance <= 100000) ? AccountType.Bronze :
                   (Balance > 100000) ? AccountType.Gold : AccountType.NotSupported;
        }
    }
}
