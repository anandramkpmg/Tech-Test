using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Database.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }
    }
}
