using CoreBanking.Services.Database.Enum;

namespace CoreBanking.Services.Business.Accounts
{
    public abstract class Account
    {
        public decimal MinBalance { get; set; }
        public decimal MaxBalance { get; set; }
        public abstract AccountType Type { get; }
    }

    public class Silver : Account
    {
        public Silver(decimal minBalance, decimal maxBalance)
        {
            MinBalance = minBalance;
            MaxBalance = maxBalance;
        }

        public override AccountType Type => AccountType.Silver;
    }

    public class Bronze : Account
    {
        public Bronze(decimal minBalance, decimal maxBalance)
        {
            MinBalance = minBalance;
            MaxBalance = maxBalance;
        }

        public override AccountType Type => AccountType.Bronze;
    }

    public class Gold : Account
    {
        public Gold(decimal minBalance, decimal maxBalance)
        {
            MinBalance = minBalance;
            MaxBalance = maxBalance;
        }

        public override AccountType Type => AccountType.Gold;
    }
}
