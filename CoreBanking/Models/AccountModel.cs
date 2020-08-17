using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }
        public string Address { get; set; }
    }
}
