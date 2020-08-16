using CoreBanking.Database.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBanking.Database.Models
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
