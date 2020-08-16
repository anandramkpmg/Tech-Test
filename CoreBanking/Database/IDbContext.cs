using System.Threading;
using System.Threading.Tasks;
using CoreBanking.Services.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Services.Database
{
    public interface IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
